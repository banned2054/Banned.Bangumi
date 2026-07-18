using System.Net;

namespace Banned.Bangumi.Test;

[TestFixture]
[Category("Integration")]
public sealed class BangumiIntegrationTests
{
    [SetUp]
    public void RequireExplicitOptIn()
    {
#if !NET10_0
        Assert.Ignore("Live integration tests run only once, on the net10.0 target.");
#endif

        if (Environment.GetEnvironmentVariable("BANGUMI_RUN_INTEGRATION_TESTS") != "1")
        {
            Assert.Ignore("Set BANGUMI_RUN_INTEGRATION_TESTS=1 to run live API tests.");
        }
    }

    [Test]
    public async Task Calendar_Get_FromDefaultEndpoint()
    {
        using var client   = CreateClient();
        var       calendar = await client.Calendar.Get();

        Assert.That(calendar, Is.Not.Empty);
    }

    [Test]
    public async Task Users_GetCurrent_WithAccessToken()
    {
        var accessToken = Environment.GetEnvironmentVariable("BANGUMI_ACCESS_TOKEN");
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            Assert.Ignore("Set BANGUMI_ACCESS_TOKEN to run authenticated live API tests.");
        }

        using var client = CreateClient(accessToken);
        var       user   = await client.Users.GetCurrent();

        Assert.That(user.Username, Is.Not.Empty);
    }

    private static BangumiClient CreateClient(string? accessToken = null)
    {
        var userAgent = Environment.GetEnvironmentVariable("BANGUMI_USER_AGENT");
        if (string.IsNullOrWhiteSpace(userAgent))
        {
            throw new InvalidOperationException("BANGUMI_USER_AGENT must be configured for live API tests.");
        }

        var proxyAddress = Environment.GetEnvironmentVariable("BANGUMI_PROXY");
        var proxy = string.IsNullOrWhiteSpace(proxyAddress)
                        ? null
                        : new WebProxy(new Uri(proxyAddress, UriKind.Absolute));

        return new BangumiClient(new BangumiClientOptions
        {
            UserAgent   = userAgent,
            AccessToken = accessToken,
            Proxy       = proxy
        });
    }
}
