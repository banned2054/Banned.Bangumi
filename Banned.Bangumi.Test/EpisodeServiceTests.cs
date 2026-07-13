using System.Net;
using System.Text;
using Banned.Bangumi.Exceptions;
using Banned.Bangumi.Models.Episodes;
using Banned.Bangumi.Models.Enums;

namespace Banned.Bangumi.Test;

public sealed class EpisodeServiceTests
{
    [Test]
    public async Task Browse_SendsPagingAndTypeFiltersAndDeserializesPage()
    {
        var handler = CreateJsonHandler("""
            {"total":2,"limit":20,"offset":10,"data":[{"id":100,"type":1,"name":"Special","name_cn":"特别篇","sort":1.5,"ep":null,"airdate":"2026-07-13","comment":7,"duration":"24m","desc":"Description","disc":0,"duration_seconds":1440}]}
            """);
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient, "optional-token");

        var result = await client.Episodes.Browse(new EpisodeBrowseRequest
        {
            SubjectId = 42,
            Type = EpisodeType.Special,
            Limit = 20,
            Offset = 10
        });

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        var episode = result.Data.Single();
        Assert.Multiple(() =>
        {
            Assert.That(request!.Method, Is.EqualTo(HttpMethod.Get));
            Assert.That(request.Uri, Is.EqualTo(
                new Uri("https://api.example/root/v0/episodes?subject_id=42&type=1&limit=20&offset=10")));
            Assert.That(request.Authorization, Is.EqualTo("Bearer optional-token"));
            Assert.That(request.Body, Is.Null);
            Assert.That(result.Total, Is.EqualTo(2));
            Assert.That(result.Limit, Is.EqualTo(20));
            Assert.That(result.Offset, Is.EqualTo(10));
            Assert.That(episode.Id, Is.EqualTo(100));
            Assert.That(episode.Type, Is.EqualTo(EpisodeType.Special));
            Assert.That(episode.EpisodeNumber, Is.Null);
            Assert.That(episode.DurationSeconds, Is.EqualTo(1440));
        });
    }

    [Test]
    public async Task Browse_OmitsOptionalFiltersAndAuthenticationWhenNotConfigured()
    {
        var handler = CreateJsonHandler("{\"total\":0,\"limit\":100,\"offset\":0,\"data\":[]}");
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient);

        var result = await client.Episodes.Browse(new EpisodeBrowseRequest { SubjectId = 42 });

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(request!.Uri, Is.EqualTo(
                new Uri("https://api.example/root/v0/episodes?subject_id=42")));
            Assert.That(request.Authorization, Is.Null);
            Assert.That(result.Data, Is.Empty);
        });
    }

    [Test]
    public async Task Get_SendsEpisodePathAndDeserializesDetails()
    {
        var handler = CreateJsonHandler("""
            {"id":100,"type":0,"name":"Episode 1","name_cn":"第一话","sort":1,"ep":1,"airdate":"2026-07-13","comment":9,"duration":"00:24:00","desc":"Description","disc":0,"subject_id":42}
            """);
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient, "optional-token");

        var result = await client.Episodes.Get(100);

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(request!.Method, Is.EqualTo(HttpMethod.Get));
            Assert.That(request.Uri, Is.EqualTo(new Uri("https://api.example/root/v0/episodes/100")));
            Assert.That(request.Authorization, Is.EqualTo("Bearer optional-token"));
            Assert.That(result.Id, Is.EqualTo(100));
            Assert.That(result.Type, Is.EqualTo(EpisodeType.MainStory));
            Assert.That(result.EpisodeNumber, Is.EqualTo(1));
            Assert.That(result.SubjectId, Is.EqualTo(42));
        });
    }

    [Test]
    public void Get_MapsNotFoundResponseToApiException()
    {
        var handler = CreateJsonHandler(
            "{\"title\":\"Not Found\",\"description\":\"Episode does not exist\"}",
            HttpStatusCode.NotFound);
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient);

        var exception = Assert.ThrowsAsync<BangumiApiException>(async () => await client.Episodes.Get(999));

        Assert.Multiple(() =>
        {
            Assert.That(exception!.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("Episode does not exist"));
        });
    }

    [Test]
    public void Browse_PropagatesCancellationToken()
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
            async () => await client.Episodes.Browse(
                new EpisodeBrowseRequest { SubjectId = 42 },
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
            Assert.That(async () => await client.Episodes.Browse(null!),
                Throws.TypeOf<ArgumentNullException>());
            Assert.That(async () => await client.Episodes.Browse(new EpisodeBrowseRequest()),
                Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Episodes.Browse(new EpisodeBrowseRequest
            {
                SubjectId = 42,
                Type = (EpisodeType)999
            }), Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Episodes.Browse(new EpisodeBrowseRequest
            {
                SubjectId = 42,
                Limit = 201
            }), Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Episodes.Browse(new EpisodeBrowseRequest
            {
                SubjectId = 42,
                Offset = -1
            }), Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Episodes.Get(0),
                Throws.TypeOf<ArgumentOutOfRangeException>());
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
