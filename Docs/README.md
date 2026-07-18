# Banned.Bangumi

[English](../README.md) | 简体中文

[![NuGet](https://img.shields.io/nuget/v/Banned.Bangumi.svg)](https://www.nuget.org/packages/Banned.Bangumi) [![Downloads](https://img.shields.io/nuget/dt/Banned.Bangumi.svg)](https://www.nuget.org/packages/Banned.Bangumi) [![License](https://img.shields.io/badge/license-Apache_2.0-green)](../LICENSE.txt)

**Banned.Bangumi** 是基于 [Bangumi OpenAPI](https://github.com/bangumi/api) 的现代强类型 .NET SDK。它以 `BangumiClient` 为唯一入口，覆盖项目内置规范中的全部 56 个操作，并支持 .NET 8、.NET 9 和 .NET 10。

## ✨ 核心特性

- **完整的 API 覆盖**：实现日历、条目、章节、角色、人物、用户、收藏、修订和目录的全部操作。
- **面向资源的设计**：将接口划分为职责明确的 Service，并统一由 `BangumiClient` 提供。
- **异步优先**：所有网络操作均返回 `Task` 或 `Task<T>`，并接受 `CancellationToken`。
- **鉴权语义明确**：区分无需 Token、可选 Token 和必须使用 Bearer Access Token 的接口。
- **一致的公开模型**：使用具有业务含义的请求类型和统一的 `PagedResult<T>`，不暴露生成器导向的 Schema 名称。
- **安全的 HTTP 所有权**：SDK 不会修改或释放调用方传入的 `HttpClient`。
- **内置代理支持**：无需自定义 `HttpClient` 即可配置 HTTP(S) 代理。
- **多目标框架支持**：支持 `net8.0`、`net9.0` 和 `net10.0`。
- **双语 API 文档**：所有公开 API 均提供简体中文和英文 XML 文档。

## 📦 安装

通过 NuGet 包管理器安装：

```bash
dotnet add package Banned.Bangumi
```

从本仓库源码使用时，也可以直接引用项目：

```xml
<ProjectReference Include="..\Banned.Bangumi\Banned.Bangumi.csproj" />
```

## 🚀 快速上手

1. 初始化客户端

每个请求都必须携带符合 Bangumi API 要求的 `User-Agent`。建议包含应用名称、版本以及可联系到开发者的地址。

```csharp
using Banned.Bangumi;

using var client = new BangumiClient(new BangumiClientOptions
{
    UserAgent = "MyBangumiApp/1.0 (https://example.com/contact)"
});

var subject = await client.Subjects.Get(253);
Console.WriteLine($"{subject.Name} / {subject.NameCn}");
```

默认 API 地址为 `https://api.bgm.tv`，单次请求默认超时时间为 30 秒。

2. 搜索条目

搜索参数使用强类型请求模型。分页值会作为 Query 参数发送，搜索条件则序列化到 JSON 请求体中。

```csharp
using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Subjects;

var results = await client.Subjects.Search(new SubjectSearchRequest
{
    Keyword = "攻壳机动队",
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
    Console.WriteLine($"{item.Id}: {item.NameCn}");
}
```

`NsfwFilter` 使用明确的三态语义：`null` 表示不限制，`NsfwFilterMode.Exclude` 表示仅返回非 NSFW 结果，`NsfwFilterMode.Only` 表示仅返回 NSFW 结果。对于没有权限的用户，服务端会忽略该筛选条件且不会返回 NSFW 内容。

3. 鉴权并更新收藏

当前用户、收藏写入和目录写入操作必须配置 Bangumi Access Token。SDK 会在单次请求上添加 Bearer Token，不会改写 `HttpClient.DefaultRequestHeaders`。

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
    Tags = ["科幻"]
});

Console.WriteLine($"已以 {me.Username} 的身份更新收藏");
```

不要把 Access Token 写入源码或日志。未配置 Token 就调用必须鉴权的接口时，SDK 会在发送请求前抛出 `BangumiAuthenticationException`。

4. 读取分页结果

分页接口统一返回 `PagedResult<T>`，包含 `Total`、`Limit`、`Offset` 和非空的 `Data` 集合。

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

## ⚙️ 配置与错误处理

`CancellationToken` 始终是网络方法的最后一个参数。取消请求或请求超时会抛出 `OperationCanceledException`；API 错误统一映射为 `BangumiApiException`，鉴权失败和限流分别使用更具体的派生异常。

```csharp
using Banned.Bangumi.Exceptions;

using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(10));

try
{
    var calendar = await client.Calendar.Get(cancellation.Token);
}
catch (BangumiRateLimitException exception)
{
    Console.WriteLine($"请求受限，建议等待：{exception.RetryAfter}");
}
catch (BangumiApiException exception)
{
    Console.WriteLine($"Bangumi API 返回 {exception.StatusCode}: {exception.Error?.Description}");
}
```

使用 SDK 内部管理的 HTTP 客户端时，可以直接配置 HTTP(S) 代理：

```csharp
using System.Net;

using var proxiedClient = new BangumiClient(new BangumiClientOptions
{
    UserAgent = "MyBangumiApp/1.0 (dev@example.com)",
    Proxy = new WebProxy(new Uri("http://127.0.0.1:7890"))
});
```

`WebProxy` 还支持凭据和绕过规则。需要接入 `IHttpClientFactory` 或其他自定义 Handler 时，仍可传入现有的
`HttpClient`：

```csharp
using var httpClient = new HttpClient(customHandler);
using var customClient = new BangumiClient(new BangumiClientOptions
{
    UserAgent = "MyBangumiApp/1.0 (dev@example.com)",
    HttpClient = httpClient,
    Timeout = TimeSpan.FromSeconds(15)
});
```

`Proxy` 与 `HttpClient` 不能同时配置，因为 SDK 无法安全地重新配置调用方客户端的消息处理程序。
`BangumiClient.Dispose()` 只会释放由 SDK 自己创建的 HTTP 客户端，调用方传入的客户端始终由调用方管理。
`BaseAddress` 也可以改为兼容的 API 网关或测试服务器，但必须是绝对 HTTP(S) 地址。

## 🛠 项目架构

| Client 属性 | 说明 | 操作数 |
| --- | --- | ---: |
| `Calendar` | 每日放送日历 | 1 |
| `Subjects` | 条目浏览、详情、图片、关联资源与搜索 | 7 |
| `Episodes` | 章节列表与详情 | 2 |
| `Characters` | 角色详情、图片、关联资源、搜索与收藏 | 7 |
| `Persons` | 人物详情、图片、关联资源、搜索与收藏 | 7 |
| `Users` | 用户资料、头像与当前用户 | 3 |
| `Collections` | 条目、章节、角色和人物收藏 | 12 |
| `Revisions` | 人物、角色、条目和章节修订记录 | 8 |
| `Indices` | 目录、目录条目及目录收藏 | 9 |

图片与头像方法（例如 `Subjects.GetImageUri`）返回 API 的重定向目标 `Uri`，不会下载图片内容。

所有功能都从同一个客户端进入：

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

方法名会利用所在 Service 的资源上下文，因此保持为简洁的 `Get`、`Search` 和 `Browse`，不添加 `Async` 后缀。IDE 中的 XML 文档会说明每个方法的参数、返回值和预期异常。

## 🧪 开发与验证

```bash
dotnet build Banned.Bangumi.slnx
dotnet test Banned.Bangumi.slnx
```

单元测试使用可控的 `HttpMessageHandler`，不依赖真实 Bangumi API。可选运行的集成测试使用默认 API 地址，
并读取环境变量 `BANGUMI_USER_AGENT`、可选的 `BANGUMI_PROXY` 和可选的 `BANGUMI_ACCESS_TOKEN`。
在 Visual Studio 中选择已被 Git 忽略的 `local.runsettings`，然后运行 `Integration` 分类即可。为避免同一请求
发送三次，真实测试只在 .NET 10 目标上执行。SDK 和测试项目均覆盖 .NET 8、.NET 9 与 .NET 10。

## 📜 更新日志

[🧾 查看 CHANGELOG](https://github.com/banned2054/Banned.Bangumi/blob/main/Docs/CHANGELOG.md)

## ⚖️ 开源协议

Copyright (c) 2026 banned.

本项目基于 Apache License 2.0 协议开源。详情请参阅 [LICENSE](../LICENSE.txt) 文件。上游归属与项目独立性声明请参阅 [NOTICE](../NOTICE)。

## 🤝 参与贡献

欢迎任何形式的贡献。如果您发现 Bug 或有新功能建议，请提交 Issue，我们也期待您的 Pull Request。

---

本项目基于 [Bangumi OpenAPI 规范](https://github.com/bangumi/api) 构建。
