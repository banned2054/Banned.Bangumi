using Banned.Bangumi.Exceptions;
using Banned.Bangumi.Models.Characters;
using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Subjects;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Banned.Bangumi.Test;

[TestFixture]
public sealed class CharacterServiceTests
{
    [Test]
    public async Task GetAndRelatedResources_SendExpectedPathsWithoutAuthenticationAndDeserializeModels()
    {
        var handler = new TestHttpMessageHandler((request, _) =>
        {
            var json = request.RequestUri!.AbsolutePath switch
            {
                "/root/v0/characters/8" => """
                    {"id":8,"name":"Hero","type":1,"summary":"Summary","locked":false,"gender":"女","blood_type":2,"stat":{"comments":3,"collects":4}}
                    """,
                "/root/v0/characters/8/subjects" => """
                    [{"id":42,"type":2,"staff":"主角","eps":"1-12","name":"Subject","name_cn":"条目","image":"https://example.test/subject.jpg"}]
                    """,
                "/root/v0/characters/8/persons" => """
                    [{"id":7,"name":"Voice Actor","type":1,"subject_id":42,"subject_type":2,"subject_name":"Subject","subject_name_cn":"条目","staff":"CV"}]
                    """,
                _ => throw new AssertionException($"Unexpected request path: {request.RequestUri.AbsolutePath}")
            };

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });
        });
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "must-not-be-sent");

        var character = await client.Characters.Get(8);
        var subjects  = await client.Characters.GetSubjects(8);
        var persons   = await client.Characters.GetPersons(8);

        var requests = handler.Requests.ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(requests.Select(request => request.Uri.AbsolutePath), Is.EqualTo(new[]
            {
                "/root/v0/characters/8",
                "/root/v0/characters/8/subjects",
                "/root/v0/characters/8/persons"
            }));
            Assert.That(requests, Has.All.Property(nameof(CapturedRequest.Method)).EqualTo(HttpMethod.Get));
            Assert.That(requests, Has.All.Property(nameof(CapturedRequest.Authorization)).Null);
            Assert.That(character.Type, Is.EqualTo(CharacterType.Character));
            Assert.That(character.BloodType, Is.EqualTo(BloodType.B));
            Assert.That(character.Statistics.CollectionCount, Is.EqualTo(4));
            Assert.That(subjects.Single().Episodes, Is.EqualTo("1-12"));
            Assert.That(subjects.Single().Type, Is.EqualTo(SubjectType.Anime));
            Assert.That(persons.Single().SubjectId, Is.EqualTo(42));
            Assert.That(persons.Single().Staff, Is.EqualTo("CV"));
        });
    }

    [Test]
    public async Task GetImageUri_SendsOptionalAuthenticationAndReturnsLocation()
    {
        var imageUri = new Uri("https://lain.example/pic/crt/l/8.jpg");
        var handler = new TestHttpMessageHandler((_, _) =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.Found);
            response.Headers.Location = imageUri;
            return Task.FromResult(response);
        });
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "optional-token");

        var result = await client.Characters.GetImageUri(8, ImageSize.Large);

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(imageUri));
            Assert.That(request!.Method, Is.EqualTo(HttpMethod.Get));
            Assert.That(request.Uri, Is.EqualTo(
                                                new Uri("https://api.example/root/v0/characters/8/image?type=large")));
            Assert.That(request.Authorization, Is.EqualTo("Bearer optional-token"));
            Assert.That(request.Body, Is.Null);
        });
    }

    [Test]
    public async Task Search_SendsPagingAndJsonBodyWithoutAuthentication()
    {
        var handler = CreateJsonHandler("""
                                        {"total":1,"limit":10,"offset":20,"data":[{"id":8,"name":"Hero","type":1,"summary":"Summary","locked":false,"stat":{"comments":3,"collects":4}}]}
                                        """);
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "must-not-be-sent");

        var result = await client.Characters.Search(new CharacterSearchRequest
        {
            Keyword = "主角",
            Filter  = new CharacterSearchFilter { NsfwFilter = NsfwFilterMode.Exclude },
            Limit   = 10,
            Offset  = 20
        });

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        using var body = JsonDocument.Parse(request!.Body!);
        var       root = body.RootElement;
        Assert.Multiple(() =>
        {
            Assert.That(request.Method, Is.EqualTo(HttpMethod.Post));
            Assert.That(request.Uri, Is.EqualTo(
                                                new
                                                    Uri("https://api.example/root/v0/search/characters?limit=10&offset=20")));
            Assert.That(request.Authorization, Is.Null);
            Assert.That(request.ContentType, Does.StartWith("application/json"));
            Assert.That(root.GetProperty("keyword").GetString(), Is.EqualTo("主角"));
            Assert.That(root.GetProperty("filter").GetProperty("nsfw").GetBoolean(), Is.False);
            Assert.That(root.TryGetProperty("limit", out _), Is.False);
            Assert.That(root.TryGetProperty("offset", out _), Is.False);
            Assert.That(result.Total, Is.EqualTo(1));
            Assert.That(result.Data.Single().Id, Is.EqualTo(8));
        });
    }

    [Test]
    public async Task AddToCollectionAndRemoveFromCollection_SendRequiredAuthenticationAndNoBody()
    {
        var handler = new TestHttpMessageHandler((_, _) =>
                                                     Task.FromResult(new HttpResponseMessage(HttpStatusCode
                                                                                 .NoContent)));
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "required-token");

        await client.Characters.AddToCollection(8);
        await client.Characters.RemoveFromCollection(8);

        var requests = handler.Requests.ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(requests.Select(request => request.Method),
                        Is.EqualTo(new[] { HttpMethod.Post, HttpMethod.Delete }));
            Assert.That(requests.Select(request => request.Uri), Is.EqualTo(new[]
            {
                new Uri("https://api.example/root/v0/characters/8/collect"),
                new Uri("https://api.example/root/v0/characters/8/collect")
            }));
            Assert.That(requests, Has.All.Property(nameof(CapturedRequest.Authorization))
                                     .EqualTo("Bearer required-token"));
            Assert.That(requests, Has.All.Property(nameof(CapturedRequest.Body)).Null);
        });
    }

    [Test]
    public void AddToCollectionAndRemoveFromCollection_RequireAccessTokenBeforeSendingRequests()
    {
        var       handler    = CreateJsonHandler("{}");
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient);

        Assert.Multiple(() =>
        {
            Assert.That(async () => await client.Characters.AddToCollection(8),
                        Throws.TypeOf<BangumiAuthenticationException>());
            Assert.That(async () => await client.Characters.RemoveFromCollection(8),
                        Throws.TypeOf<BangumiAuthenticationException>());
        });
        Assert.That(handler.Requests, Is.Empty);
    }

    [Test]
    public void Get_MapsNotFoundResponseToApiException()
    {
        var handler = CreateJsonHandler(
                                        "{\"title\":\"Not Found\",\"description\":\"Character does not exist\"}",
                                        HttpStatusCode.NotFound);
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient);

        var exception = Assert.ThrowsAsync<BangumiApiException>(async () => await client.Characters.Get(999));

        Assert.Multiple(() =>
        {
            Assert.That(exception!.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("Character does not exist"));
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
        using var httpClient         = new HttpClient(handler);
        using var client             = CreateClient(httpClient);
        using var cancellationSource = new CancellationTokenSource();
        cancellationSource.Cancel();

        Assert.That(
                    async () => await client.Characters.Search(
                                                               new CharacterSearchRequest { Keyword = "Hero" },
                                                               cancellationSource.Token),
                    Throws.InstanceOf<OperationCanceledException>());
    }

    [Test]
    public void PublicMethods_RejectInvalidArgumentsBeforeSendingRequests()
    {
        var       handler    = CreateJsonHandler("{}");
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "token");

        Assert.Multiple(() =>
        {
            Assert.That(async () => await client.Characters.Get(0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Characters.GetImageUri(8, (ImageSize)999),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Characters.GetImageUri(8, ImageSize.Common),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Characters.GetSubjects(0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Characters.GetPersons(0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Characters.Search(null!),
                        Throws.TypeOf<ArgumentNullException>());
            Assert.That(async () => await client.Characters.Search(
                                                                   new CharacterSearchRequest { Keyword = " " }),
                        Throws.TypeOf<ArgumentException>());
            Assert.That(async () => await client.Characters.Search(new CharacterSearchRequest
            {
                Keyword = "Hero",
                Limit   = 0
            }), Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Characters.Search(new CharacterSearchRequest
            {
                Keyword = "Hero",
                Offset  = -1
            }), Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Characters.AddToCollection(0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Characters.RemoveFromCollection(0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
        });
        Assert.That(handler.Requests, Is.Empty);
    }

    private static TestHttpMessageHandler CreateJsonHandler(
        string         json,
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
            UserAgent   = "Banned.Bangumi.Test/1.0",
            HttpClient  = httpClient
        });
}
