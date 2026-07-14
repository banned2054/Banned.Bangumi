using System.Net;
using System.Text;
using System.Text.Json;
using Banned.Bangumi.Exceptions;
using Banned.Bangumi.Models.Characters;
using Banned.Bangumi.Models.Persons;
using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Subjects;

namespace Banned.Bangumi.Test;

[TestFixture]
public sealed class SubjectServiceTests
{
    [Test]
    public async Task Browse_SendsAllQueryParametersAndDeserializesPage()
    {
        const string responseJson = """
            {
              "total": 1,
              "limit": 20,
              "offset": 40,
              "data": [
                {
                  "id": 42,
                  "type": 2,
                  "name": "Example",
                  "name_cn": "示例",
                  "summary": "Summary",
                  "series": false,
                  "nsfw": false,
                  "locked": false,
                  "date": "2026-07-13",
                  "platform": "TV",
                  "images": {
                    "large": "https://lain.example/l.jpg",
                    "common": "https://lain.example/c.jpg",
                    "medium": "https://lain.example/m.jpg",
                    "small": "https://lain.example/s.jpg",
                    "grid": "https://lain.example/g.jpg"
                  },
                  "infobox": [{"key": "中文名", "value": "示例"}],
                  "volumes": 0,
                  "eps": 12,
                  "total_episodes": 12,
                  "rating": {
                    "rank": 10,
                    "total": 100,
                    "count": {"1": 1, "10": 50},
                    "score": 8.5
                  },
                  "collection": {
                    "wish": 1,
                    "collect": 2,
                    "doing": 3,
                    "on_hold": 4,
                    "dropped": 5
                  },
                  "meta_tags": ["原创"],
                  "tags": [{"name": "科幻", "count": 9}]
                }
              ]
            }
            """;
        var handler = CreateJsonHandler(responseJson);
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient, accessToken: "optional-token");

        var result = await client.Subjects.Browse(new SubjectBrowseRequest
        {
            Type = SubjectType.Anime,
            Category = SubjectCategory.AnimeTelevision,
            Series = true,
            Platform = "TV / Web",
            Sort = SubjectBrowseSort.Rank,
            Year = 2026,
            Month = 7,
            Limit = 20,
            Offset = 40
        });

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        var subject = result.Data.Single();
        Assert.Multiple(() =>
        {
            Assert.That(request!.Method, Is.EqualTo(HttpMethod.Get));
            Assert.That(request.Uri, Is.EqualTo(new Uri(
                "https://api.example/root/v0/subjects?type=2&cat=1&series=true&platform=TV%20%2F%20Web&sort=rank&year=2026&month=7&limit=20&offset=40")));
            Assert.That(request.UserAgent, Is.EqualTo("Banned.Bangumi.Test/1.0"));
            Assert.That(request.Authorization, Is.EqualTo("Bearer optional-token"));
            Assert.That(request.Body, Is.Null);
            Assert.That(result.Total, Is.EqualTo(1));
            Assert.That(result.Limit, Is.EqualTo(20));
            Assert.That(result.Offset, Is.EqualTo(40));
            Assert.That(subject.Id, Is.EqualTo(42));
            Assert.That(subject.Type, Is.EqualTo(SubjectType.Anime));
            Assert.That(subject.Episodes, Is.EqualTo(12));
            Assert.That(subject.Rating.Score, Is.EqualTo(8.5));
            Assert.That(subject.Rating.Count.Score10, Is.EqualTo(50));
            Assert.That(subject.Collection.OnHold, Is.EqualTo(4));
            Assert.That(subject.Tags.Single().Name, Is.EqualTo("科幻"));
            Assert.That(subject.Tags.Single().Count, Is.EqualTo(9));
            Assert.That(subject.Tags.Single(), Is.TypeOf<Tag>());
            Assert.That(subject.Infobox!.Single().Value.GetString(), Is.EqualTo("示例"));
        });
    }

    [Test]
    public async Task GetAndRelatedResources_SendExpectedPathsAndDeserializeModels()
    {
        var handler = new TestHttpMessageHandler((request, _) =>
        {
            var json = request.RequestUri!.AbsolutePath switch
            {
                "/root/v0/subjects/42" => """
                    {"id":42,"type":2,"name":"Example","name_cn":"示例","summary":"","series":false,"nsfw":false,"locked":false,"platform":"TV","images":{},"volumes":0,"eps":12,"total_episodes":12,"rating":{"rank":0,"total":0,"count":{},"score":0},"collection":{"wish":0,"collect":0,"doing":0,"on_hold":0,"dropped":0},"meta_tags":[],"tags":[]}
                    """,
                "/root/v0/subjects/42/persons" => """
                    [{"id":7,"name":"Voice Actor","type":1,"career":["seiyu"],"relation":"声优","eps":"1"}]
                    """,
                "/root/v0/subjects/42/characters" => """
                    [{"id":8,"name":"Hero","summary":"","type":1,"relation":"主角","actors":[{"id":7,"name":"Voice Actor","type":1,"career":["actor"],"short_summary":"","locked":false}]}]
                    """,
                "/root/v0/subjects/42/subjects" => """
                    [{"id":9,"type":2,"name":"Next","name_cn":"续作","relation":"续集"}]
                    """,
                _ => throw new AssertionException($"Unexpected request path: {request.RequestUri.AbsolutePath}")
            };

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });
        });
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient, accessToken: "optional-token");

        var subject = await client.Subjects.Get(42);
        var persons = await client.Subjects.GetPersons(42);
        var characters = await client.Subjects.GetCharacters(42);
        var relations = await client.Subjects.GetRelations(42);

        var requests = handler.Requests.ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(requests.Select(request => request.Uri.AbsolutePath), Is.EqualTo(new[]
            {
                "/root/v0/subjects/42",
                "/root/v0/subjects/42/persons",
                "/root/v0/subjects/42/characters",
                "/root/v0/subjects/42/subjects"
            }));
            Assert.That(requests, Has.All.Property(nameof(CapturedRequest.Method)).EqualTo(HttpMethod.Get));
            Assert.That(requests, Has.All.Property(nameof(CapturedRequest.Authorization)).EqualTo("Bearer optional-token"));
            Assert.That(subject.NameCn, Is.EqualTo("示例"));
            Assert.That(persons.Single().Careers.Single(), Is.EqualTo(PersonCareer.Seiyu));
            Assert.That(characters.Single().Type, Is.EqualTo(CharacterType.Character));
            Assert.That(characters.Single().Actors.Single().Careers.Single(), Is.EqualTo(PersonCareer.Actor));
            Assert.That(relations.Single().Relation, Is.EqualTo("续集"));
        });
    }

    [Test]
    public async Task GetImageUri_ReturnsLocationWithoutDownloadingImage()
    {
        var imageUri = new Uri("https://lain.example/pic/cover/l/42.jpg");
        var handler = new TestHttpMessageHandler((_, _) =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.Found);
            response.Headers.Location = imageUri;
            return Task.FromResult(response);
        });
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient, accessToken: "optional-token");

        var result = await client.Subjects.GetImageUri(42, ImageSize.Large);

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(imageUri));
            Assert.That(request!.Method, Is.EqualTo(HttpMethod.Get));
            Assert.That(request.Uri, Is.EqualTo(
                new Uri("https://api.example/root/v0/subjects/42/image?type=large")));
            Assert.That(request.Authorization, Is.EqualTo("Bearer optional-token"));
            Assert.That(request.Body, Is.Null);
        });
    }

    [Test]
    public async Task Search_SendsQueryAndJsonBodyWithoutAuthentication()
    {
        var handler = CreateJsonHandler("""
            {"total":1,"limit":10,"offset":20,"data":[{"id":42,"type":2,"name":"Example","name_cn":"示例","summary":"","series":false,"nsfw":false,"locked":false,"platform":"TV","images":{},"volumes":0,"eps":0,"total_episodes":0,"rating":{"rank":0,"total":0,"count":{},"score":0},"collection":{"wish":0,"collect":0,"doing":0,"on_hold":0,"dropped":0},"meta_tags":[],"tags":[]}]}
            """);
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient, accessToken: "must-not-be-sent");

        var result = await client.Subjects.Search(new SubjectSearchRequest
        {
            Keyword = "星际 牛仔",
            Sort = SubjectSearchSort.Score,
            Limit = 10,
            Offset = 20,
            Filter = new SubjectSearchFilter
            {
                Types = [SubjectType.Anime, SubjectType.LiveAction],
                MetaTags = ["原创", "-科幻"],
                Ratings = [">=6", "<9"],
                Nsfw = false
            }
        });

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        using var body = JsonDocument.Parse(request!.Body!);
        var root = body.RootElement;
        var filter = root.GetProperty("filter");
        Assert.Multiple(() =>
        {
            Assert.That(request.Method, Is.EqualTo(HttpMethod.Post));
            Assert.That(request.Uri, Is.EqualTo(
                new Uri("https://api.example/root/v0/search/subjects?limit=10&offset=20")));
            Assert.That(request.Authorization, Is.Null);
            Assert.That(request.ContentType, Does.StartWith("application/json"));
            Assert.That(root.GetProperty("keyword").GetString(), Is.EqualTo("星际 牛仔"));
            Assert.That(root.GetProperty("sort").GetString(), Is.EqualTo("score"));
            Assert.That(root.TryGetProperty("limit", out _), Is.False);
            Assert.That(root.TryGetProperty("offset", out _), Is.False);
            Assert.That(filter.GetProperty("type").EnumerateArray().Select(value => value.GetInt32()),
                Is.EqualTo(new[] { 2, 6 }));
            Assert.That(filter.GetProperty("meta_tags").EnumerateArray().Select(value => value.GetString()),
                Is.EqualTo(new[] { "原创", "-科幻" }));
            Assert.That(filter.GetProperty("nsfw").GetBoolean(), Is.False);
            Assert.That(result.Total, Is.EqualTo(1));
            Assert.That(result.Data.Single().Id, Is.EqualTo(42));
        });
    }

    [Test]
    public void Get_MapsNotFoundResponseToApiException()
    {
        var handler = CreateJsonHandler(
            "{\"title\":\"Not Found\",\"description\":\"Subject does not exist\"}",
            HttpStatusCode.NotFound);
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient);

        var exception = Assert.ThrowsAsync<BangumiApiException>(async () => await client.Subjects.Get(999));

        Assert.Multiple(() =>
        {
            Assert.That(exception!.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("Subject does not exist"));
        });
    }

    [Test]
    public void Search_PropagatesCancellationToken()
    {
        var handler = new TestHttpMessageHandler(async (_, cancellationToken) =>
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            throw new InvalidOperationException("The request should have been cancelled.");
        });
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient);
        using var cancellationSource = new CancellationTokenSource();
        cancellationSource.Cancel();

        Assert.That(
            async () => await client.Subjects.Search(
                new SubjectSearchRequest { Keyword = "test" },
                cancellationSource.Token),
            Throws.InstanceOf<OperationCanceledException>());
    }

    [Test]
    public void PublicMethods_RejectInvalidArgumentsBeforeSendingRequests()
    {
        var handler = CreateJsonHandler("{}");
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient);

        Assert.Multiple(() =>
        {
            Assert.That(async () => await client.Subjects.Get(0),
                Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Subjects.Browse(new SubjectBrowseRequest
            {
                Type = SubjectType.Anime,
                Limit = 51
            }), Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Subjects.Search(
                new SubjectSearchRequest { Keyword = " " }), Throws.TypeOf<ArgumentException>());
        });
        Assert.That(handler.Requests, Is.Empty);
    }

    private static TestHttpMessageHandler CreateJsonHandler(
        string json,
        HttpStatusCode statusCode = HttpStatusCode.OK) =>
        new((_, _) => Task.FromResult(new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        }));

    private static BangumiClient CreateClient(HttpClient httpClient, string? accessToken = null) =>
        new(new BangumiClientOptions
        {
            BaseAddress = new Uri("https://api.example/root"),
            AccessToken = accessToken,
            UserAgent = "Banned.Bangumi.Test/1.0",
            HttpClient = httpClient
        });
}
