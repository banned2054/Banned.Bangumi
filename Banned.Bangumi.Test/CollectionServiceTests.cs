using Banned.Bangumi.Exceptions;
using Banned.Bangumi.Models.Characters;
using Banned.Bangumi.Models.Collections;
using Banned.Bangumi.Models.Episodes;
using Banned.Bangumi.Models.Persons;
using Banned.Bangumi.Models.Subjects;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Banned.Bangumi.Test;

[TestFixture]
public sealed class CollectionServiceTests
{
    [Test]
    public async Task BrowseSubjects_SendsFiltersAndOptionalAuthenticationAndDeserializesModels()
    {
        var handler = CreateJsonHandler("""
                                        {
                                          "total":1,
                                          "limit":20,
                                          "offset":10,
                                          "data":[{
                                            "subject_id":42,
                                            "subject_type":2,
                                            "rate":9,
                                            "type":3,
                                            "comment":"很好看",
                                            "tags":["动画","推理"],
                                            "ep_status":12,
                                            "vol_status":0,
                                            "updated_at":"2024-01-02T03:04:05+08:00",
                                            "private":true,
                                            "subject":{
                                              "id":42,
                                              "type":2,
                                              "name":"Subject",
                                              "name_cn":"条目",
                                              "short_summary":"Summary",
                                              "date":"2024-01-01",
                                              "images":{"large":"https://example.test/large.jpg"},
                                              "volumes":0,
                                              "eps":12,
                                              "collection_total":100,
                                              "score":8.8,
                                              "rank":10,
                                              "tags":[{"name":"推理","count":5}]
                                            }
                                          }]
                                        }
                                        """);
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "optional-token");

        var result = await client.Collections.BrowseSubjects("user/name", new SubjectCollectionBrowseRequest
        {
            SubjectType = SubjectType.Anime,
            Type        = SubjectCollectionType.Doing,
            Limit       = 20,
            Offset      = 10
        });

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        var collection = result.Data.Single();
        Assert.Multiple(() =>
        {
            Assert.That(request!.Method, Is.EqualTo(HttpMethod.Get));
            Assert.That(request.Uri, Is.EqualTo(new Uri(
                            "https://api.example/root/v0/users/user%2Fname/collections?subject_type=2&type=3&limit=20&offset=10")));
            Assert.That(request.Authorization, Is.EqualTo("Bearer optional-token"));
            Assert.That(request.Body, Is.Null);
            Assert.That(result.Total, Is.EqualTo(1));
            Assert.That(collection.SubjectId, Is.EqualTo(42));
            Assert.That(collection.SubjectType, Is.EqualTo(SubjectType.Anime));
            Assert.That(collection.Type, Is.EqualTo(SubjectCollectionType.Doing));
            Assert.That(collection.Tags, Is.EqualTo(new[] { "动画", "推理" }));
            Assert.That(collection.IsPrivate, Is.True);
            Assert.That(collection.Subject!.Type, Is.EqualTo(SubjectType.Anime));
            Assert.That(collection.Subject.CollectionCount, Is.EqualTo(100));
            Assert.That(collection.Subject.Tags.Single().Name, Is.EqualTo("推理"));
        });
    }

    [Test]
    public async Task GetSubject_SendsEscapedPathWithOptionalAuthenticationDisabled()
    {
        var handler = CreateJsonHandler("""
                                        {"subject_id":42,"subject_type":2,"rate":0,"type":1,"tags":[],"ep_status":0,"vol_status":0,"updated_at":"2024-01-02T03:04:05Z","private":false}
                                        """);
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient);

        var result = await client.Collections.GetSubject("user name", 42);

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(request!.Method, Is.EqualTo(HttpMethod.Get));
            Assert.That(request.Uri,
                        Is.EqualTo(new Uri("https://api.example/root/v0/users/user%20name/collections/42")));
            Assert.That(request.Authorization, Is.Null);
            Assert.That(result.Type, Is.EqualTo(SubjectCollectionType.Wish));
            Assert.That(result.Subject, Is.Null);
        });
    }

    [Test]
    public async Task CreateOrUpdateSubjectAndUpdateSubject_SendRequiredAuthenticationAndJsonBodies()
    {
        var handler = CreateNoContentHandler();
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "required-token");

        await client.Collections.CreateOrUpdateSubject(42, new SubjectCollectionUpdateRequest
        {
            Type          = SubjectCollectionType.Doing,
            Rate          = 9,
            EpisodeStatus = 12,
            VolumeStatus  = 2,
            Comment       = "很好看",
            IsPrivate     = true,
            Tags          = ["动画", "推理"]
        });
        await client.Collections.UpdateSubject(42, new SubjectCollectionUpdateRequest { Comment = "更新评价" });

        var requests = handler.Requests.ToArray();
        using var createBody = JsonDocument.Parse(requests[0].Body!);
        using var updateBody = JsonDocument.Parse(requests[1].Body!);
        var createRoot = createBody.RootElement;
        var updateRoot = updateBody.RootElement;
        Assert.Multiple(() =>
        {
            Assert.That(requests.Select(request => request.Method),
                        Is.EqualTo(new[] { HttpMethod.Post, HttpMethod.Patch }));
            Assert.That(requests.Select(request => request.Uri), Is.EqualTo(new[]
            {
                new Uri("https://api.example/root/v0/users/-/collections/42"),
                new Uri("https://api.example/root/v0/users/-/collections/42")
            }));
            Assert.That(requests, Has.All.Property(nameof(CapturedRequest.Authorization))
                                     .EqualTo("Bearer required-token"));
            Assert.That(requests, Has.All.Property(nameof(CapturedRequest.ContentType))
                                     .StartsWith("application/json"));
            Assert.That(createRoot.GetProperty("type").GetInt32(), Is.EqualTo(3));
            Assert.That(createRoot.GetProperty("rate").GetInt32(), Is.EqualTo(9));
            Assert.That(createRoot.GetProperty("ep_status").GetInt32(), Is.EqualTo(12));
            Assert.That(createRoot.GetProperty("vol_status").GetInt32(), Is.EqualTo(2));
            Assert.That(createRoot.GetProperty("private").GetBoolean(), Is.True);
            Assert.That(createRoot.GetProperty("tags").EnumerateArray().Select(value => value.GetString()),
                        Is.EqualTo(new[] { "动画", "推理" }));
            Assert.That(updateRoot.EnumerateObject().Select(property => property.Name),
                        Is.EqualTo(new[] { "comment" }));
            Assert.That(updateRoot.GetProperty("comment").GetString(), Is.EqualTo("更新评价"));
        });
    }

    [Test]
    public async Task BrowseEpisodes_SendsFiltersAndRequiredAuthenticationAndDeserializesModels()
    {
        var handler = CreateJsonHandler("""
                                        {
                                          "total":1,
                                          "limit":100,
                                          "offset":20,
                                          "data":[{
                                            "episode":{"id":7,"type":0,"name":"Episode","name_cn":"章节","sort":1,"ep":1,"airdate":"2024-01-01","comment":2,"duration":"24m","desc":"Description","disc":0,"duration_seconds":1440},
                                            "type":2,
                                            "updated_at":1700000000
                                          }]
                                        }
                                        """);
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "required-token");

        var result = await client.Collections.BrowseEpisodes(42, new EpisodeCollectionBrowseRequest
        {
            EpisodeType = EpisodeType.MainStory,
            Limit       = 100,
            Offset      = 20
        });

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        var collection = result.Data.Single();
        Assert.Multiple(() =>
        {
            Assert.That(request!.Method, Is.EqualTo(HttpMethod.Get));
            Assert.That(request.Uri, Is.EqualTo(new Uri(
                            "https://api.example/root/v0/users/-/collections/42/episodes?offset=20&limit=100&episode_type=0")));
            Assert.That(request.Authorization, Is.EqualTo("Bearer required-token"));
            Assert.That(collection.Episode.Id, Is.EqualTo(7));
            Assert.That(collection.Episode.Type, Is.EqualTo(EpisodeType.MainStory));
            Assert.That(collection.Type, Is.EqualTo(EpisodeCollectionType.Done));
            Assert.That(collection.UpdatedAt, Is.EqualTo(1700000000));
        });
    }

    [Test]
    public async Task UpdateEpisodesAndUpdateEpisode_SendRequiredAuthenticationAndExpectedJsonBodies()
    {
        var handler = CreateNoContentHandler();
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "required-token");

        await client.Collections.UpdateEpisodes(42, new EpisodeCollectionBatchUpdateRequest
        {
            EpisodeIds = [7, 8],
            Type       = EpisodeCollectionType.Done
        });
        await client.Collections.UpdateEpisode(7, new EpisodeCollectionUpdateRequest
        {
            Type = EpisodeCollectionType.Dropped
        });

        var requests = handler.Requests.ToArray();
        using var batchBody  = JsonDocument.Parse(requests[0].Body!);
        using var singleBody = JsonDocument.Parse(requests[1].Body!);
        Assert.Multiple(() =>
        {
            Assert.That(requests.Select(request => request.Method),
                        Is.EqualTo(new[] { HttpMethod.Patch, HttpMethod.Put }));
            Assert.That(requests.Select(request => request.Uri), Is.EqualTo(new[]
            {
                new Uri("https://api.example/root/v0/users/-/collections/42/episodes"),
                new Uri("https://api.example/root/v0/users/-/collections/-/episodes/7")
            }));
            Assert.That(requests, Has.All.Property(nameof(CapturedRequest.Authorization))
                                     .EqualTo("Bearer required-token"));
            Assert.That(batchBody.RootElement.GetProperty("episode_id").EnumerateArray()
                                 .Select(value => value.GetInt32()), Is.EqualTo(new[] { 7, 8 }));
            Assert.That(batchBody.RootElement.GetProperty("type").GetInt32(), Is.EqualTo(2));
            Assert.That(singleBody.RootElement.GetProperty("type").GetInt32(), Is.EqualTo(3));
        });
    }

    [Test]
    public async Task GetEpisode_SendsRequiredAuthenticationAndDeserializesModel()
    {
        var handler = CreateJsonHandler("""
                                        {"episode":{"id":7,"type":1,"name":"Special","name_cn":"特别篇","sort":1,"ep":null,"airdate":"","comment":0,"duration":"","desc":"","disc":0,"duration_seconds":null},"type":1,"updated_at":0}
                                        """);
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "required-token");

        var result = await client.Collections.GetEpisode(7);

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(request!.Method, Is.EqualTo(HttpMethod.Get));
            Assert.That(request.Uri,
                        Is.EqualTo(new Uri("https://api.example/root/v0/users/-/collections/-/episodes/7")));
            Assert.That(request.Authorization, Is.EqualTo("Bearer required-token"));
            Assert.That(result.Episode.Type, Is.EqualTo(EpisodeType.Special));
            Assert.That(result.Type, Is.EqualTo(EpisodeCollectionType.Wish));
            Assert.That(result.UpdatedAt, Is.Zero);
        });
    }

    [Test]
    public async Task GetCharactersAndGetCharacter_SendPublicPathsWithoutAuthenticationAndDeserializeModels()
    {
        var handler = new TestHttpMessageHandler((request, _) =>
        {
            var json = request.RequestUri!.AbsolutePath.EndsWith("/characters", StringComparison.Ordinal)
                ? """
                  {"total":1,"limit":30,"offset":0,"data":[{"id":8,"name":"Hero","type":1,"images":{"small":"https://example.test/character.jpg"},"created_at":"2024-01-02T03:04:05Z"}]}
                  """
                : """
                  {"id":8,"name":"Hero","type":1,"images":null,"created_at":"2024-01-02T03:04:05Z"}
                  """;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });
        });
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "must-not-be-sent");

        var list = await client.Collections.GetCharacters("user/name");
        var item = await client.Collections.GetCharacter("user/name", 8);

        var requests = handler.Requests.ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(requests.Select(request => request.Uri), Is.EqualTo(new[]
            {
                new Uri("https://api.example/root/v0/users/user%2Fname/collections/-/characters"),
                new Uri("https://api.example/root/v0/users/user%2Fname/collections/-/characters/8")
            }));
            Assert.That(requests, Has.All.Property(nameof(CapturedRequest.Method)).EqualTo(HttpMethod.Get));
            Assert.That(requests, Has.All.Property(nameof(CapturedRequest.Authorization)).Null);
            Assert.That(list.Data.Single().Type, Is.EqualTo(CharacterType.Character));
            Assert.That(list.Data.Single().Images!.Small, Is.EqualTo("https://example.test/character.jpg"));
            Assert.That(item.Id, Is.EqualTo(8));
            Assert.That(item.Images, Is.Null);
        });
    }

    [Test]
    public async Task GetPersonsAndGetPerson_SendPublicPathsWithoutAuthenticationAndDeserializeModels()
    {
        var handler = new TestHttpMessageHandler((request, _) =>
        {
            var json = request.RequestUri!.AbsolutePath.EndsWith("/persons", StringComparison.Ordinal)
                ? """
                  {"total":1,"limit":30,"offset":0,"data":[{"id":9,"name":"Voice Actor","type":1,"career":["seiyu","actor"],"images":{"medium":"https://example.test/person.jpg"},"created_at":"2024-01-02T03:04:05Z"}]}
                  """
                : """
                  {"id":9,"name":"Voice Actor","type":1,"career":["seiyu"],"created_at":"2024-01-02T03:04:05Z"}
                  """;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });
        });
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "must-not-be-sent");

        var list = await client.Collections.GetPersons("user");
        var item = await client.Collections.GetPerson("user", 9);

        var requests = handler.Requests.ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(requests.Select(request => request.Uri), Is.EqualTo(new[]
            {
                new Uri("https://api.example/root/v0/users/user/collections/-/persons"),
                new Uri("https://api.example/root/v0/users/user/collections/-/persons/9")
            }));
            Assert.That(requests, Has.All.Property(nameof(CapturedRequest.Authorization)).Null);
            Assert.That(list.Data.Single().Type, Is.EqualTo(PersonType.Individual));
            Assert.That(list.Data.Single().Careers, Is.EqualTo(new[] { PersonCareer.Seiyu, PersonCareer.Actor }));
            Assert.That(item.Careers.Single(), Is.EqualTo(PersonCareer.Seiyu));
        });
    }

    [Test]
    public void CurrentUserOperations_RequireAccessTokenBeforeSendingRequests()
    {
        var       handler    = CreateJsonHandler("{}");
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient);

        Assert.Multiple(() =>
        {
            Assert.That(async () => await client.Collections.CreateOrUpdateSubject(
                            42, new SubjectCollectionUpdateRequest()),
                        Throws.TypeOf<BangumiAuthenticationException>());
            Assert.That(async () => await client.Collections.UpdateSubject(
                            42, new SubjectCollectionUpdateRequest()),
                        Throws.TypeOf<BangumiAuthenticationException>());
            Assert.That(async () => await client.Collections.BrowseEpisodes(42),
                        Throws.TypeOf<BangumiAuthenticationException>());
            Assert.That(async () => await client.Collections.UpdateEpisodes(
                            42, new EpisodeCollectionBatchUpdateRequest
                            {
                                EpisodeIds = [7],
                                Type       = EpisodeCollectionType.Done
                            }), Throws.TypeOf<BangumiAuthenticationException>());
            Assert.That(async () => await client.Collections.GetEpisode(7),
                        Throws.TypeOf<BangumiAuthenticationException>());
            Assert.That(async () => await client.Collections.UpdateEpisode(
                            7, new EpisodeCollectionUpdateRequest { Type = EpisodeCollectionType.Done }),
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
            Assert.That(async () => await client.Collections.BrowseSubjects(" "),
                        Throws.TypeOf<ArgumentException>());
            Assert.That(async () => await client.Collections.BrowseSubjects("user", new()
            {
                SubjectType = (SubjectType)5
            }), Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Collections.BrowseSubjects("user", new() { Limit = 51 }),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Collections.GetSubject("user", 0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Collections.CreateOrUpdateSubject(
                            42, new SubjectCollectionUpdateRequest { Rate = 11 }),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Collections.UpdateSubject(
                            42, new SubjectCollectionUpdateRequest { Tags = ["has space"] }),
                        Throws.TypeOf<ArgumentException>());
            Assert.That(async () => await client.Collections.BrowseEpisodes(42, new() { Limit = 1001 }),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Collections.UpdateEpisodes(
                            42, new EpisodeCollectionBatchUpdateRequest
                            {
                                EpisodeIds = [0],
                                Type       = EpisodeCollectionType.Done
                            }), Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Collections.GetEpisode(0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Collections.UpdateEpisode(
                            7, new EpisodeCollectionUpdateRequest { Type = (EpisodeCollectionType)99 }),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Collections.GetCharacter("user", 0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Collections.GetPerson("user", 0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
        });
        Assert.That(handler.Requests, Is.Empty);
    }

    [Test]
    public void BrowseSubjects_PropagatesCancellationToken()
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

        Assert.That(async () => await client.Collections.BrowseSubjects(
                        "user", cancellationToken : cancellationSource.Token),
                    Throws.InstanceOf<OperationCanceledException>());
    }

    [Test]
    public void BrowseSubjects_MapsUnauthorizedResponseToAuthenticationException()
    {
        var handler = CreateJsonHandler(
            "{\"title\":\"Unauthorized\",\"description\":\"Private collection\"}",
            HttpStatusCode.Unauthorized);
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "invalid-token");

        var exception = Assert.ThrowsAsync<BangumiAuthenticationException>(
            async () => await client.Collections.BrowseSubjects("user"));

        Assert.Multiple(() =>
        {
            Assert.That(exception!.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            Assert.That(exception.Message, Is.EqualTo("Private collection"));
        });
    }

    private static TestHttpMessageHandler CreateJsonHandler(
        string         json,
        HttpStatusCode statusCode = HttpStatusCode.OK) =>
        new((_, _) => Task.FromResult(new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        }));

    private static TestHttpMessageHandler CreateNoContentHandler() =>
        new((_, _) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.NoContent)));

    private static BangumiClient CreateClient(HttpClient httpClient, string? accessToken = null) =>
        new(new BangumiClientOptions
        {
            BaseAddress = new Uri("https://api.example/root"),
            AccessToken = accessToken,
            UserAgent   = "Banned.Bangumi.Test/1.0",
            HttpClient  = httpClient
        });
}
