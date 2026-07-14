using Banned.Bangumi.Exceptions;
using Banned.Bangumi.Models.Indices;
using Banned.Bangumi.Models.Subjects;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Banned.Bangumi.Test;

[TestFixture]
public sealed class IndexServiceTests
{
    [Test]
    public async Task Create_SendsRequiredAuthenticationWithoutBodyAndDeserializesIndex()
    {
        var handler = CreateJsonHandler(CreateIndexJson());
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "required-token");

        var result = await client.Indices.Create();

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(request!.Method, Is.EqualTo(HttpMethod.Post));
            Assert.That(request.Uri, Is.EqualTo(new Uri("https://api.example/root/v0/indices")));
            Assert.That(request.Authorization, Is.EqualTo("Bearer required-token"));
            Assert.That(request.Body, Is.Null);
            Assert.That(result.Id, Is.EqualTo(42));
            Assert.That(result.Title, Is.EqualTo("测试目录"));
            Assert.That(result.Description, Is.EqualTo("目录描述"));
            Assert.That(result.Total, Is.EqualTo(3));
            Assert.That(result.Statistics.CommentCount, Is.EqualTo(5));
            Assert.That(result.Statistics.CollectionCount, Is.EqualTo(8));
            Assert.That(result.Creator.Username, Is.EqualTo("sai"));
            Assert.That(result.CreatedAt.Offset, Is.EqualTo(TimeSpan.FromHours(8)));
            Assert.That(result.UpdatedAt, Is.EqualTo(DateTimeOffset.Parse("2026-07-14T10:00:00+08:00")));
            Assert.That(result.Nsfw, Is.True);
        });
    }

    [Test]
    public async Task Get_SendsOptionalAuthenticationAndDeserializesIndex()
    {
        var handler = CreateJsonHandler(CreateIndexJson());
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "optional-token");

        var result = await client.Indices.Get(42);

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(request!.Method, Is.EqualTo(HttpMethod.Get));
            Assert.That(request.Uri, Is.EqualTo(new Uri("https://api.example/root/v0/indices/42")));
            Assert.That(request.Authorization, Is.EqualTo("Bearer optional-token"));
            Assert.That(request.Body, Is.Null);
            Assert.That(result.Id, Is.EqualTo(42));
        });
    }

    [Test]
    public async Task Update_SendsRequiredAuthenticationAndJsonBodyAndDeserializesIndex()
    {
        var handler = CreateJsonHandler(CreateIndexJson());
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "required-token");

        var result = await client.Indices.Update(42, new IndexUpdateRequest
        {
            Title       = "新标题",
            Description = "新描述"
        });

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        using var body = JsonDocument.Parse(request!.Body!);
        Assert.Multiple(() =>
        {
            Assert.That(request.Method, Is.EqualTo(HttpMethod.Put));
            Assert.That(request.Uri, Is.EqualTo(new Uri("https://api.example/root/v0/indices/42")));
            Assert.That(request.Authorization, Is.EqualTo("Bearer required-token"));
            Assert.That(request.ContentType, Does.StartWith("application/json"));
            Assert.That(body.RootElement.EnumerateObject().Select(property => property.Name),
                        Is.EqualTo(new[] { "title", "description" }));
            Assert.That(body.RootElement.GetProperty("title").GetString(), Is.EqualTo("新标题"));
            Assert.That(body.RootElement.GetProperty("description").GetString(), Is.EqualTo("新描述"));
            Assert.That(result.Id, Is.EqualTo(42));
        });
    }

    [Test]
    public async Task GetSubjects_SendsFiltersWithoutAuthenticationAndDeserializesPage()
    {
        var handler = CreateJsonHandler("""
                                        {
                                          "total": 1,
                                          "limit": 20,
                                          "offset": 10,
                                          "data": [{
                                            "id": 7,
                                            "type": 2,
                                            "name": "Test Subject",
                                            "images": { "small": "https://example.test/subject.jpg" },
                                            "infobox": [{ "key": "导演", "value": [{ "v": "测试导演" }] }],
                                            "date": "2026-07-14",
                                            "comment": "推荐",
                                            "added_at": "2026-07-14T09:30:00+08:00"
                                          }]
                                        }
                                        """);
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient);

        var result = await client.Indices.GetSubjects(42, SubjectType.Anime, 20, 10);

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        var subject = result.Data.Single();
        var infobox = subject.Infobox!;
        Assert.Multiple(() =>
        {
            Assert.That(request!.Method, Is.EqualTo(HttpMethod.Get));
            Assert.That(request.Uri, Is.EqualTo(new Uri(
                            "https://api.example/root/v0/indices/42/subjects?type=2&limit=20&offset=10")));
            Assert.That(request.Authorization, Is.Null);
            Assert.That(request.Body, Is.Null);
            Assert.That(result.Total, Is.EqualTo(1));
            Assert.That(result.Limit, Is.EqualTo(20));
            Assert.That(result.Offset, Is.EqualTo(10));
            Assert.That(subject.Id, Is.EqualTo(7));
            Assert.That(subject.Type, Is.EqualTo(SubjectType.Anime));
            Assert.That(subject.Images!.Small, Is.EqualTo("https://example.test/subject.jpg"));
            Assert.That(infobox.Single().Key, Is.EqualTo("导演"));
            Assert.That(infobox.Single().Value.ValueKind, Is.EqualTo(JsonValueKind.Array));
            Assert.That(subject.Comment, Is.EqualTo("推荐"));
            Assert.That(subject.AddedAt.Offset, Is.EqualTo(TimeSpan.FromHours(8)));
        });
    }

    [Test]
    public async Task SubjectWrites_SendExpectedPathsAuthenticationAndBodies()
    {
        var handler = CreateNoContentHandler();
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "required-token");

        await client.Indices.AddSubject(42, new IndexSubjectAddRequest
        {
            SubjectId = 7,
            Sort      = 10,
            Comment   = "新增备注"
        });
        await client.Indices.CreateOrUpdateSubject(42, 7, new IndexSubjectUpdateRequest
        {
            Comment = "修改备注"
        });
        await client.Indices.RemoveSubject(42, 7);

        var requests = handler.Requests.ToArray();
        using var addBody    = JsonDocument.Parse(requests[0].Body!);
        using var updateBody = JsonDocument.Parse(requests[1].Body!);
        Assert.Multiple(() =>
        {
            Assert.That(requests.Select(request => request.Method),
                        Is.EqualTo(new[] { HttpMethod.Post, HttpMethod.Put, HttpMethod.Delete }));
            Assert.That(requests.Select(request => request.Uri), Is.EqualTo(new[]
            {
                new Uri("https://api.example/root/v0/indices/42/subjects"),
                new Uri("https://api.example/root/v0/indices/42/subjects/7"),
                new Uri("https://api.example/root/v0/indices/42/subjects/7")
            }));
            Assert.That(requests, Has.All.Property(nameof(CapturedRequest.Authorization))
                                     .EqualTo("Bearer required-token"));
            Assert.That(addBody.RootElement.GetProperty("subject_id").GetInt32(), Is.EqualTo(7));
            Assert.That(addBody.RootElement.GetProperty("sort").GetInt32(), Is.EqualTo(10));
            Assert.That(addBody.RootElement.GetProperty("comment").GetString(), Is.EqualTo("新增备注"));
            Assert.That(updateBody.RootElement.EnumerateObject().Select(property => property.Name),
                        Is.EqualTo(new[] { "comment" }));
            Assert.That(updateBody.RootElement.GetProperty("comment").GetString(), Is.EqualTo("修改备注"));
            Assert.That(requests[2].Body, Is.Null);
        });
    }

    [Test]
    public async Task AddToCollectionAndRemoveFromCollection_SendRequiredAuthenticationWithoutBodies()
    {
        var handler = CreateNoContentHandler();
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "required-token");

        await client.Indices.AddToCollection(42);
        await client.Indices.RemoveFromCollection(42);

        var requests = handler.Requests.ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(requests.Select(request => request.Method),
                        Is.EqualTo(new[] { HttpMethod.Post, HttpMethod.Delete }));
            Assert.That(requests, Has.All.Property(nameof(CapturedRequest.Uri))
                                     .EqualTo(new Uri("https://api.example/root/v0/indices/42/collect")));
            Assert.That(requests, Has.All.Property(nameof(CapturedRequest.Authorization))
                                     .EqualTo("Bearer required-token"));
            Assert.That(requests, Has.All.Property(nameof(CapturedRequest.Body)).Null);
        });
    }

    [Test]
    public void WriteOperations_RequireAccessTokenBeforeSendingRequests()
    {
        var       handler    = CreateJsonHandler("{}");
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient);

        Assert.Multiple(() =>
        {
            Assert.That(async () => await client.Indices.Create(),
                        Throws.TypeOf<BangumiAuthenticationException>());
            Assert.That(async () => await client.Indices.Update(42, new IndexUpdateRequest()),
                        Throws.TypeOf<BangumiAuthenticationException>());
            Assert.That(async () => await client.Indices.AddSubject(42, new IndexSubjectAddRequest { SubjectId = 7 }),
                        Throws.TypeOf<BangumiAuthenticationException>());
            Assert.That(async () => await client.Indices.CreateOrUpdateSubject(
                            42, 7, new IndexSubjectUpdateRequest()),
                        Throws.TypeOf<BangumiAuthenticationException>());
            Assert.That(async () => await client.Indices.RemoveSubject(42, 7),
                        Throws.TypeOf<BangumiAuthenticationException>());
            Assert.That(async () => await client.Indices.AddToCollection(42),
                        Throws.TypeOf<BangumiAuthenticationException>());
            Assert.That(async () => await client.Indices.RemoveFromCollection(42),
                        Throws.TypeOf<BangumiAuthenticationException>());
        });
        Assert.That(handler.Requests, Is.Empty);
    }

    [Test]
    public void PublicMethods_RejectInvalidArgumentsBeforeSendingRequests()
    {
        var       handler    = CreateJsonHandler("{}");
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "token");

        Assert.Multiple(() =>
        {
            Assert.That(async () => await client.Indices.Get(0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Indices.Update(0, new IndexUpdateRequest()),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Indices.Update(42, null!),
                        Throws.TypeOf<ArgumentNullException>());
            Assert.That(async () => await client.Indices.GetSubjects(42, (SubjectType)5),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Indices.GetSubjects(42, limit : 0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Indices.GetSubjects(42, offset : -1),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Indices.AddSubject(
                            42, new IndexSubjectAddRequest { SubjectId = 0 }),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Indices.CreateOrUpdateSubject(
                            42, 0, new IndexSubjectUpdateRequest()),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Indices.RemoveSubject(0, 7),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Indices.AddToCollection(0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
        });
        Assert.That(handler.Requests, Is.Empty);
    }

    [Test]
    public void Get_MapsNotFoundResponseToApiException()
    {
        var handler = CreateJsonHandler(
                                        "{\"title\":\"Not Found\",\"description\":\"Index does not exist\"}",
                                        HttpStatusCode.NotFound);
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient);

        var exception = Assert.ThrowsAsync<BangumiApiException>(async () => await client.Indices.Get(999));

        Assert.Multiple(() =>
        {
            Assert.That(exception!.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("Index does not exist"));
        });
    }

    [Test]
    public void GetSubjects_PropagatesCancellationToken()
    {
        var handler = new TestHttpMessageHandler(async (_, cancellationToken) =>
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            throw new InvalidOperationException("The request should have been cancelled.");
        });
        using var httpClient         = new HttpClient(handler);
        using var client             = CreateClient(httpClient);
        using var cancellationSource = new CancellationTokenSource();
        cancellationSource.Cancel();

        Assert.That(async () => await client.Indices.GetSubjects(
                        42, cancellationToken : cancellationSource.Token),
                    Throws.InstanceOf<OperationCanceledException>());
    }

    private static string CreateIndexJson() =>
        """
        {
          "id": 42,
          "title": "测试目录",
          "desc": "目录描述",
          "total": 3,
          "stat": { "comments": 5, "collects": 8 },
          "created_at": "2026-07-14T09:00:00+08:00",
          "updated_at": "2026-07-14T10:00:00+08:00",
          "creator": { "username": "sai", "nickname": "Sai" },
          "ban": false,
          "nsfw": true
        }
        """;

    private static TestHttpMessageHandler CreateJsonHandler(
        string         json,
        HttpStatusCode statusCode = HttpStatusCode.OK) =>
        new((_, _) => Task.FromResult(new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        }));

    private static TestHttpMessageHandler CreateNoContentHandler() =>
        new((_, _) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

    private static BangumiClient CreateClient(HttpClient httpClient, string? accessToken = null) =>
        new(new BangumiClientOptions
        {
            BaseAddress = new Uri("https://api.example/root"),
            AccessToken = accessToken,
            UserAgent   = "Banned.Bangumi.Test/1.0",
            HttpClient  = httpClient
        });
}
