using Banned.Bangumi.Services.Internal;

namespace Banned.Bangumi.Test;

[TestFixture]
public sealed class BangumiClientTests
{
    [Test]
    public void Constructor_CreatesAllServicePropertiesWithSharedHttpService()
    {
        using var client = new BangumiClient(new BangumiClientOptions
        {
            UserAgent = "Banned.Bangumi.Test/1.0"
        });

        Assert.Multiple(() =>
        {
            Assert.That(client.Calendar, Is.Not.Null);
            Assert.That(client.Subjects, Is.Not.Null);
            Assert.That(client.Episodes, Is.Not.Null);
            Assert.That(client.Characters, Is.Not.Null);
            Assert.That(client.Persons, Is.Not.Null);
            Assert.That(client.Users, Is.Not.Null);
            Assert.That(client.Collections, Is.Not.Null);
            Assert.That(client.Revisions, Is.Not.Null);
            Assert.That(client.Indices, Is.Not.Null);
            Assert.That(client.Subjects.HttpService, Is.SameAs(client.Calendar.HttpService));
            Assert.That(client.Indices.HttpService, Is.SameAs(client.Calendar.HttpService));
        });
    }

    [TestCase("")]
    [TestCase("   ")]
    [TestCase("invalid\r\nvalue")]
    public void Constructor_RejectsInvalidUserAgent(string userAgent)
    {
        var options = new BangumiClientOptions { UserAgent = userAgent };

        Assert.That(
                    () => new BangumiClient(options),
                    Throws.TypeOf<ArgumentException>());
    }

    [Test]
    public void Constructor_RejectsInvalidBaseAddressAndTimeout()
    {
        var invalidAddress = new BangumiClientOptions
        {
            BaseAddress = new Uri("file:///tmp/bangumi"),
            UserAgent   = "Banned.Bangumi.Test/1.0"
        };
        var invalidTimeout = new BangumiClientOptions
        {
            UserAgent = "Banned.Bangumi.Test/1.0",
            Timeout   = TimeSpan.Zero
        };

        Assert.Multiple(() =>
        {
            Assert.That(() => new BangumiClient(invalidAddress), Throws.TypeOf<ArgumentException>());
            Assert.That(() => new BangumiClient(invalidTimeout), Throws.TypeOf<ArgumentOutOfRangeException>());
        });
    }

    [Test]
    public async Task Dispose_DoesNotDisposeCallerOwnedHttpClient()
    {
        var handler = new TestHttpMessageHandler((_, _) => Task.FromResult(new HttpResponseMessage
        {
            StatusCode = System.Net.HttpStatusCode.NoContent
        }));
        using var httpClient = new HttpClient(handler);
        var client = new BangumiClient(new BangumiClientOptions
        {
            UserAgent  = "Banned.Bangumi.Test/1.0",
            HttpClient = httpClient
        });

        client.Dispose();
        using var response = await httpClient.GetAsync("https://example.test/ping");

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NoContent));
            Assert.That(handler.IsDisposed, Is.False);
        });
    }

    [Test]
    public void HttpService_OwnsInternallyCreatedHttpClientAndRejectsUseAfterDispose()
    {
        var service = new BangumiHttpService(new BangumiClientOptions
        {
            UserAgent = "Banned.Bangumi.Test/1.0"
        });

        Assert.That(service.OwnsHttpClient, Is.True);
        service.Dispose();

        Assert.That(async () => await service.Send(HttpMethod.Get, "/test", AuthenticationMode.None),
                    Throws.TypeOf<ObjectDisposedException>());
    }
}
