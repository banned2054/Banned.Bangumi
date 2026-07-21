# Banned.Bangumi

English | [简体中文](./Docs/README.md)

[![NuGet](https://img.shields.io/nuget/v/Banned.Bangumi.svg)](https://www.nuget.org/packages/Banned.Bangumi) [![Downloads](https://img.shields.io/nuget/dt/Banned.Bangumi.svg)](https://www.nuget.org/packages/Banned.Bangumi) [![License](https://img.shields.io/badge/license-Apache_2.0-green)](./LICENSE.txt)

**Banned.Bangumi** is a user-friendly, strongly typed .NET SDK for the [Bangumi API](https://github.com/bangumi/api), with Native AOT and trimming support. Its models are derived from the official OpenAPI specification and carefully curated instead of exposing mechanically generated names and structures. A resource-oriented API available through one `BangumiClient` covers all 56 operations in the bundled specification and supports .NET 8, .NET 9, and .NET 10.

## ✨ Key Features

- **Full API Coverage**: Implements all operations for calendars, subjects, episodes, characters, persons, users, collections, revisions, and indices.
- **Resource-Oriented Design**: Organizes endpoints into focused services available from one `BangumiClient`.
- **Asynchronous First**: Every network operation returns `Task` or `Task<T>` and accepts a `CancellationToken`.
- **Authentication-Aware**: Distinguishes endpoints that require no token, accept an optional token, or require a Bearer access token.
- **Curated OpenAPI Models**: Renames and reshapes specification-derived models into meaningful resource types, including a shared `PagedResult<T>`.
- **Safe HTTP Ownership**: A caller-provided `HttpClient` is never modified or disposed by the SDK.
- **Built-in Proxy Support**: Configures HTTP(S) proxies without requiring a custom `HttpClient`.
- **Multi-Target Support**: Builds for `net8.0`, `net9.0`, and `net10.0`.
- **Native AOT Compatible**: Uses source-generated JSON metadata and supports trimming and Native AOT publishing.
- **Bilingual API Documentation**: All public APIs include Simplified Chinese and English XML documentation.

## 📦 Installation

Install via NuGet Package Manager:

```bash
dotnet add package Banned.Bangumi
```

To use the project directly from source:

```xml
<ProjectReference Include="..\Banned.Bangumi\Banned.Bangumi.csproj" />
```

## 🚀 Quick Start

1. Initialize the Client

Every request must include a `User-Agent` that follows the Bangumi API requirements. It should identify your application and provide a way to contact its developer.

```csharp
using Banned.Bangumi;

using var client = new BangumiClient(new BangumiClientOptions
{
    UserAgent = "MyBangumiApp/1.0 (https://example.com/contact)"
});

var subject = await client.Subjects.Get(253);
Console.WriteLine($"{subject.Name} / {subject.NameCn}");
```

The default API address is `https://api.bgm.tv`, and the default per-request timeout is 30 seconds.

2. Search for Subjects

Search options use strongly typed request models. Pagination values are sent as query parameters, while search criteria are serialized into the JSON request body.

```csharp
using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Subjects;

var results = await client.Subjects.Search(new SubjectSearchRequest
{
    Keyword = "Ghost in the Shell",
    Sort = SubjectSearchSort.Rank,
    Filter = new SubjectSearchFilter
    {
        Types = [SubjectType.Anime],
        NsfwFilter = NsfwFilterMode.Exclude
    },
    Limit = 20,
    Offset = 0
});

foreach (var item in results.Data)
{
    Console.WriteLine($"{item.Id}: {item.Name}");
}
```

`NsfwFilter` is explicit and tri-state: `null` applies no restriction, `NsfwFilterMode.Exclude` returns only non-NSFW results, and `NsfwFilterMode.Only` returns only NSFW results. The server ignores this filter for users without permission and does not return NSFW content.

3. Authenticate and Update a Collection

Current-user, collection write, and index write operations require a Bangumi access token. The SDK adds the Bearer token to each request without changing `HttpClient.DefaultRequestHeaders`.

```csharp
using Banned.Bangumi.Models.Collections;

using var authenticatedClient = new BangumiClient(new BangumiClientOptions
{
    UserAgent = "MyBangumiApp/1.0 (dev@example.com)",
    AccessToken = Environment.GetEnvironmentVariable("BANGUMI_ACCESS_TOKEN")
});

var me = await authenticatedClient.Users.GetCurrent();

await authenticatedClient.Collections.CreateOrUpdateSubject(253, new SubjectCollectionUpdateRequest
{
    Type = SubjectCollectionType.Doing,
    Rate = 9,
    Tags = ["science fiction"]
});

Console.WriteLine($"Updated the collection as {me.Username}");
```

Never place an access token in source code or logs. If a required-authentication operation is called without a configured token, the SDK throws `BangumiAuthenticationException` before sending the request.

4. Read Paginated Results

Paginated endpoints consistently return `PagedResult<T>`, containing `Total`, `Limit`, `Offset`, and a non-null `Data` collection.

```csharp
using Banned.Bangumi.Models.Collections;
using Banned.Bangumi.Models.Subjects;

const int pageSize = 50;
var offset = 0;

while (true)
{
    var page = await client.Collections.BrowseSubjects("sai", new SubjectCollectionBrowseRequest
    {
        SubjectType = SubjectType.Anime,
        Type = SubjectCollectionType.Done,
        Limit = pageSize,
        Offset = offset
    });

    foreach (var collection in page.Data)
    {
        Console.WriteLine(collection.Subject?.Name);
    }

    offset += page.Data.Count;
    if (page.Data.Count == 0 || offset >= page.Total)
    {
        break;
    }
}
```

## ⚙️ Configuration and Error Handling

`CancellationToken` is always the last parameter of a network method. Cancellation and request timeouts throw `OperationCanceledException`. API errors are mapped to `BangumiApiException`, with specialized exceptions for authentication failures and rate limiting.

```csharp
using Banned.Bangumi.Exceptions;

using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(10));

try
{
    var calendar = await client.Calendar.Get(cancellation.Token);
}
catch (BangumiRateLimitException exception)
{
    Console.WriteLine($"Rate limited. Suggested delay: {exception.RetryAfter}");
}
catch (BangumiApiException exception)
{
    Console.WriteLine($"Bangumi API returned {exception.StatusCode}: {exception.Error?.Description}");
}
```

An HTTP(S) proxy can be configured directly while the SDK continues to manage the HTTP client:

```csharp
using System.Net;

using var proxiedClient = new BangumiClient(new BangumiClientOptions
{
    UserAgent = "MyBangumiApp/1.0 (dev@example.com)",
    Proxy = new WebProxy(new Uri("http://127.0.0.1:7890"))
});
```

`WebProxy` also supports credentials and bypass rules. For `IHttpClientFactory` or other custom handler integration,
a caller-owned `HttpClient` can still be supplied:

```csharp
using var httpClient = new HttpClient(customHandler);
using var customClient = new BangumiClient(new BangumiClientOptions
{
    UserAgent = "MyBangumiApp/1.0 (dev@example.com)",
    HttpClient = httpClient,
    Timeout = TimeSpan.FromSeconds(15)
});
```

`Proxy` and `HttpClient` are mutually exclusive because the SDK cannot safely reconfigure a supplied client's
message handler. `BangumiClient.Dispose()` only disposes HTTP clients created internally by the SDK. A supplied
client remains entirely caller-owned. `BaseAddress` can also be changed for a compatible API gateway or test server
and must be an absolute HTTP(S) URI.

## 🛠 Project Architecture

| Client Property | Description | Operations |
| --- | --- | ---: |
| `Calendar` | Daily airing calendar | 1 |
| `Subjects` | Subject browsing, details, images, related resources, and search | 7 |
| `Episodes` | Episode listing and details | 2 |
| `Characters` | Character details, images, related resources, search, and collections | 7 |
| `Persons` | Person details, images, related resources, search, and collections | 7 |
| `Users` | Public profiles, avatars, and the current user | 3 |
| `Collections` | Subject, episode, character, and person collections | 12 |
| `Revisions` | Person, character, subject, and episode revision history | 8 |
| `Indices` | Indices, index subjects, and index collections | 9 |

Image and avatar methods such as `Subjects.GetImageUri` return the API redirect target as a `Uri`; they do not download image content.

All functionality is available from the same client:

```csharp
var calendar = await client.Calendar.Get();
var subject = await client.Subjects.Get(253);
var episode = await client.Episodes.Get(12345);
var character = await client.Characters.Get(1);
var person = await client.Persons.Get(1);
var user = await client.Users.Get("sai");
var collection = await client.Collections.GetSubject("sai", 253);
var revisions = await client.Revisions.GetSubjects(253, limit: 20);
var index = await client.Indices.Get(1);
```

Method names use their service context and therefore stay concise (`Get`, `Search`, and `Browse`) without an `Async` suffix. IntelliSense XML documentation describes parameters, return values, and expected exceptions.

## 🧪 Development

```bash
dotnet build Banned.Bangumi.slnx
dotnet test Banned.Bangumi.slnx
```

Unit tests use a controlled `HttpMessageHandler` and do not depend on the live Bangumi API. Opt-in integration tests
use the default API address and read `BANGUMI_USER_AGENT`, optional `BANGUMI_PROXY`, and optional
`BANGUMI_ACCESS_TOKEN` environment variables. Select the ignored `local.runsettings` file in Visual Studio, then run
the `Integration` category. Live tests execute only on the .NET 10 target to avoid sending each request three times.
Both the SDK and test project target .NET 8, .NET 9, and .NET 10.

Native AOT compatibility is verified with a self-contained smoke application that exercises both request serialization
and response deserialization without accessing the live API:

```bash
dotnet publish Banned.Bangumi.AotSmoke/Banned.Bangumi.AotSmoke.csproj -c Release -r linux-x64 -p:PublishAot=true
```

## 📜 Changelog

[🧾 View CHANGELOG](https://github.com/banned2054/Banned.Bangumi/blob/main/Docs/CHANGELOG.md)

## ⚖️ License

Copyright (c) 2026 banned.

This project is licensed under the Apache License 2.0. See the [LICENSE](./LICENSE.txt) file for details. Upstream attributions and project independence notices are provided in [NOTICE](./NOTICE).

## 🤝 Contributing

Contributions are welcome. If you encounter a bug or have a feature request, please open an issue. Pull requests are appreciated.

---

Built against the [Bangumi OpenAPI specification](https://github.com/bangumi/api).
