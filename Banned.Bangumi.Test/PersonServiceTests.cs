using Banned.Bangumi.Exceptions;
using Banned.Bangumi.Models.Characters;
using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Persons;
using Banned.Bangumi.Models.Subjects;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Banned.Bangumi.Test;

[TestFixture]
public sealed class PersonServiceTests
{
    [Test]
    public async Task GetAndRelatedResources_SendExpectedPathsWithoutAuthenticationAndDeserializeModels()
    {
        var handler = new TestHttpMessageHandler((request, _) =>
        {
            var json = request.RequestUri!.AbsolutePath switch
            {
                "/root/v0/persons/7" => """
                    {"id":7,"name":"Voice Actor","type":1,"career":["seiyu"],"summary":"Summary","locked":false,"last_modified":"2024-01-02T03:04:05Z","infobox":[{"key":"简体中文名","value":"声优"}],"gender":"女","blood_type":1,"birth_year":1980,"birth_mon":1,"birth_day":2,"stat":{"comments":3,"collects":4}}
                    """,
                "/root/v0/persons/7/subjects" => """
                    [{"id":42,"type":2,"staff":"声优","eps":"1-12","name":"Subject","name_cn":"条目","image":"https://example.test/subject.jpg"}]
                    """,
                "/root/v0/persons/7/characters" => """
                    [{"id":8,"name":"Hero","type":1,"subject_id":42,"subject_type":2,"subject_name":"Subject","subject_name_cn":"条目","staff":"CV"}]
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

        var person     = await client.Persons.Get(7);
        var subjects   = await client.Persons.GetSubjects(7);
        var characters = await client.Persons.GetCharacters(7);

        var requests = handler.Requests.ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(requests.Select(request => request.Uri.AbsolutePath), Is.EqualTo(new[]
            {
                "/root/v0/persons/7",
                "/root/v0/persons/7/subjects",
                "/root/v0/persons/7/characters"
            }));
            Assert.That(requests, Has.All.Property(nameof(CapturedRequest.Method)).EqualTo(HttpMethod.Get));
            Assert.That(requests, Has.All.Property(nameof(CapturedRequest.Authorization)).Null);
            Assert.That(person.Type, Is.EqualTo(PersonType.Individual));
            Assert.That(person.Careers.Single(), Is.EqualTo(PersonCareer.Seiyu));
            Assert.That(person.BloodType, Is.EqualTo(BloodType.A));
            Assert.That(person.BirthMonth, Is.EqualTo(1));
            Assert.That(person.Statistics.CollectionCount, Is.EqualTo(4));
            Assert.That(subjects.Single().Episodes, Is.EqualTo("1-12"));
            Assert.That(subjects.Single().Type, Is.EqualTo(SubjectType.Anime));
            Assert.That(characters.Single().Type, Is.EqualTo(CharacterType.Character));
            Assert.That(characters.Single().SubjectId, Is.EqualTo(42));
            Assert.That(characters.Single().Staff, Is.EqualTo("CV"));
        });
    }

    [Test]
    public async Task GetImageUri_SendsOptionalAuthenticationAndReturnsLocation()
    {
        var imageUri = new Uri("https://lain.example/pic/prsn/l/7.jpg");
        var handler = new TestHttpMessageHandler((_, _) =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.Found);
            response.Headers.Location = imageUri;
            return Task.FromResult(response);
        });
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "optional-token");

        var result = await client.Persons.GetImageUri(7, ImageSize.Large);

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(imageUri));
            Assert.That(request!.Method, Is.EqualTo(HttpMethod.Get));
            Assert.That(request.Uri,
                        Is.EqualTo(new Uri("https://api.example/root/v0/persons/7/image?type=large")));
            Assert.That(request.Authorization, Is.EqualTo("Bearer optional-token"));
            Assert.That(request.Body, Is.Null);
        });
    }

    [Test]
    public async Task Search_SendsPagingAndJsonBodyWithoutAuthentication()
    {
        var handler = CreateJsonHandler("""
                                        {"total":1,"limit":10,"offset":20,"data":[{"id":7,"name":"Voice Actor","type":1,"career":["seiyu"],"short_summary":"Summary","locked":false}]}
                                        """);
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "must-not-be-sent");

        var result = await client.Persons.Search(new PersonSearchRequest
        {
            Keyword = "声优",
            Filter  = new PersonSearchFilter { Careers = [PersonCareer.Seiyu, PersonCareer.Actor] },
            Limit   = 10,
            Offset  = 20
        });

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        using var body    = JsonDocument.Parse(request!.Body!);
        var       root    = body.RootElement;
        var       careers = root.GetProperty("filter").GetProperty("career");
        Assert.Multiple(() =>
        {
            Assert.That(request.Method, Is.EqualTo(HttpMethod.Post));
            Assert.That(request.Uri,
                        Is.EqualTo(new Uri("https://api.example/root/v0/search/persons?limit=10&offset=20")));
            Assert.That(request.Authorization, Is.Null);
            Assert.That(request.ContentType, Does.StartWith("application/json"));
            Assert.That(root.GetProperty("keyword").GetString(), Is.EqualTo("声优"));
            Assert.That(careers.EnumerateArray().Select(value => value.GetString()),
                        Is.EqualTo(new[] { "seiyu", "actor" }));
            Assert.That(root.TryGetProperty("limit", out _), Is.False);
            Assert.That(root.TryGetProperty("offset", out _), Is.False);
            Assert.That(result.Total, Is.EqualTo(1));
            Assert.That(result.Data.Single().Id, Is.EqualTo(7));
            Assert.That(result.Data.Single().Careers.Single(), Is.EqualTo(PersonCareer.Seiyu));
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

        await client.Persons.AddToCollection(7);
        await client.Persons.RemoveFromCollection(7);

        var requests = handler.Requests.ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(requests.Select(request => request.Method),
                        Is.EqualTo(new[] { HttpMethod.Post, HttpMethod.Delete }));
            Assert.That(requests.Select(request => request.Uri), Is.EqualTo(new[]
            {
                new Uri("https://api.example/root/v0/persons/7/collect"),
                new Uri("https://api.example/root/v0/persons/7/collect")
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
            Assert.That(async () => await client.Persons.AddToCollection(7),
                        Throws.TypeOf<BangumiAuthenticationException>());
            Assert.That(async () => await client.Persons.RemoveFromCollection(7),
                        Throws.TypeOf<BangumiAuthenticationException>());
        });
        Assert.That(handler.Requests, Is.Empty);
    }

    [Test]
    public void Get_MapsNotFoundResponseToApiException()
    {
        var handler = CreateJsonHandler(
                                        "{\"title\":\"Not Found\",\"description\":\"Person does not exist\"}",
                                        HttpStatusCode.NotFound);
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient);

        var exception = Assert.ThrowsAsync<BangumiApiException>(async () => await client.Persons.Get(999));

        Assert.Multiple(() =>
        {
            Assert.That(exception!.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("Person does not exist"));
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
                    async () => await client.Persons.Search(
                                                             new PersonSearchRequest { Keyword = "Voice Actor" },
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
            Assert.That(async () => await client.Persons.Get(0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Persons.GetImageUri(7, (ImageSize)999),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Persons.GetImageUri(7, ImageSize.Common),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Persons.GetSubjects(0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Persons.GetCharacters(0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Persons.Search(null!),
                        Throws.TypeOf<ArgumentNullException>());
            Assert.That(async () => await client.Persons.Search(new PersonSearchRequest { Keyword = " " }),
                        Throws.TypeOf<ArgumentException>());
            Assert.That(async () => await client.Persons.Search(new PersonSearchRequest
            {
                Keyword = "Voice Actor",
                Limit   = 0
            }), Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Persons.Search(new PersonSearchRequest
            {
                Keyword = "Voice Actor",
                Offset  = -1
            }), Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Persons.AddToCollection(0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Persons.RemoveFromCollection(0),
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
