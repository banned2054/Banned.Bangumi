using Banned.Bangumi.Exceptions;
using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Services.Internal;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Banned.Bangumi.Test;

[TestFixture]
public sealed class BangumiHttpServiceTests
{
    [Test]
    public void AddQueryString_EncodesValuesAndOmitsNulls()
    {
        var path = BangumiHttpService.AddQueryString("/v0/subjects?limit=20",
                                                     new Dictionary<string, string?>
                                                     {
                                                         ["tag"]     = "科幻 / SF",
                                                         ["empty"]   = string.Empty,
                                                         ["omitted"] = null
                                                     });

        Assert.That(path, Is.EqualTo("/v0/subjects?limit=20&tag=%E7%A7%91%E5%B9%BB%20%2F%20SF&empty="));
    }

    [Test]
    public async Task Send_AddsPerRequestHeadersAndSerializesBodyWithoutMutatingHttpClient()
    {
        var       handler    = CreateJsonHandler("{\"description\":\"response\"}");
        using var httpClient = new HttpClient(handler);
        httpClient.BaseAddress = new Uri("https://caller.example/");
        httpClient.Timeout     = TimeSpan.FromSeconds(11);
        httpClient.DefaultRequestHeaders.Add("X-Caller", "preserved");
        var service = CreateService(handler, httpClient, accessToken : "secret-token");

        var result = await service.Send<ErrorDetail, ErrorDetail>(HttpMethod.Post, "/v0/test?offset=2",
                                                                 AuthenticationMode.Optional,
                                                                 new ErrorDetail { Description = "request" });

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(result.Description, Is.EqualTo("response"));
            Assert.That(request!.Method, Is.EqualTo(HttpMethod.Post));
            Assert.That(request.Uri, Is.EqualTo(new Uri("https://api.example/root/v0/test?offset=2")));
            Assert.That(request.UserAgent, Is.EqualTo("Banned.Bangumi.Test/1.0"));
            Assert.That(request.Authorization, Is.EqualTo("Bearer secret-token"));
            Assert.That(request.ContentType, Does.StartWith("application/json"));
            Assert.That(request.Body, Is.EqualTo("{\"title\":null,\"description\":\"request\",\"details\":null}"));
            Assert.That(httpClient.BaseAddress, Is.EqualTo(new Uri("https://caller.example/")));
            Assert.That(httpClient.Timeout, Is.EqualTo(TimeSpan.FromSeconds(11)));
            Assert.That(httpClient.DefaultRequestHeaders.Contains("X-Caller"), Is.True);
            Assert.That(httpClient.DefaultRequestHeaders.UserAgent, Is.Empty);
            Assert.That(httpClient.DefaultRequestHeaders.Authorization, Is.Null);
        });
    }

    [Test]
    public async Task Send_RespectsNoneOptionalAndRequiredAuthenticationModes()
    {
        var       authenticatedHandler = CreateJsonHandler("{}");
        using var authenticatedClient  = new HttpClient(authenticatedHandler);
        using var authenticatedService =
            CreateService(authenticatedHandler, authenticatedClient, accessToken : "secret-token");

        await authenticatedService.Send(HttpMethod.Get, "/none", AuthenticationMode.None);
        await authenticatedService.Send(HttpMethod.Get, "/required", AuthenticationMode.Required);

        var requests = authenticatedHandler.Requests.ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(requests[0].Authorization, Is.Null);
            Assert.That(requests[1].Authorization, Is.EqualTo("Bearer secret-token"));
        });

        var       anonymousHandler = CreateJsonHandler("{}");
        using var anonymousClient  = new HttpClient(anonymousHandler);
        using var anonymousService = CreateService(anonymousHandler, anonymousClient);

        await anonymousService.Send(HttpMethod.Get, "/optional", AuthenticationMode.Optional);

        Assert.That(anonymousHandler.Requests.Single().Authorization, Is.Null);
        Assert.That(async () => await anonymousService.Send(HttpMethod.Get, "/required", AuthenticationMode.Required),
                    Throws.TypeOf<BangumiAuthenticationException>());
        Assert.That(anonymousHandler.Requests, Has.Count.EqualTo(1));
    }

    [Test]
    public void Send_MapsAuthenticationErrorAndDeserializesErrorBody()
    {
        var handler =
            CreateJsonHandler("{\"title\":\"Unauthorized\",\"description\":\"Token expired\",\"details\":\"renew it\"}",
                              HttpStatusCode.Unauthorized);
        using var httpClient = new HttpClient(handler);
        using var service    = CreateService(handler, httpClient, accessToken : "secret-token");

        var exception =
            Assert.ThrowsAsync<BangumiAuthenticationException>(async () => await service.Send(HttpMethod.Get,
                                                                        "/private",
                                                                        AuthenticationMode.Required));

        Assert.Multiple(() =>
        {
            Assert.That(exception!.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            Assert.That(exception.Message, Is.EqualTo("Token expired"));
            Assert.That(exception.Error?.Title, Is.EqualTo("Unauthorized"));
            Assert.That(exception.Error?.Details, Is.EqualTo("renew it"));
            Assert.That(exception.ResponseBody, Does.Contain("Token expired"));
            Assert.That(exception.ToString(), Does.Not.Contain("secret-token"));
        });
    }

    [Test]
    public void Send_MapsRateLimitAndRetryAfter()
    {
        var handler = new TestHttpMessageHandler((_, _) =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests)
            {
                Content = new StringContent("{\"description\":\"Slow down\"}", Encoding.UTF8, "application/json")
            };
            response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(17));
            return Task.FromResult(response);
        });
        using var httpClient = new HttpClient(handler);
        using var service    = CreateService(handler, httpClient);

        var exception =
            Assert.ThrowsAsync<BangumiRateLimitException>(async () =>
                                                              await service.Send(HttpMethod.Get, "/limited",
                                                                       AuthenticationMode.None));

        Assert.Multiple(() =>
        {
            Assert.That(exception!.StatusCode, Is.EqualTo(HttpStatusCode.TooManyRequests));
            Assert.That(exception.RetryAfter, Is.EqualTo(TimeSpan.FromSeconds(17)));
            Assert.That(exception.Message, Is.EqualTo("Slow down"));
        });
    }

    [Test]
    public void Send_MapsMalformedErrorWithoutLosingDiagnosticBody()
    {
        var       handler    = CreateJsonHandler("upstream failed", HttpStatusCode.BadGateway);
        using var httpClient = new HttpClient(handler);
        using var service    = CreateService(handler, httpClient);

        var exception =
            Assert.ThrowsAsync<BangumiApiException>(async () =>
                                                        await service.Send(HttpMethod.Get, "/failed",
                                                                           AuthenticationMode.None));

        Assert.Multiple(() =>
        {
            Assert.That(exception!.StatusCode, Is.EqualTo(HttpStatusCode.BadGateway));
            Assert.That(exception.Error, Is.Null);
            Assert.That(exception.ResponseBody, Is.EqualTo("upstream failed"));
        });
    }

    [Test]
    public void Get_MapsInvalidSuccessJsonToApiException()
    {
        var       handler    = CreateJsonHandler("not-json");
        using var httpClient = new HttpClient(handler);
        using var service    = CreateService(handler, httpClient);

        var exception =
            Assert.ThrowsAsync<BangumiApiException>(async () => await service.Get<ErrorDetail>("/invalid-json"));

        Assert.Multiple(() =>
        {
            Assert.That(exception!.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(exception.InnerException, Is.TypeOf<System.Text.Json.JsonException>());
            Assert.That(exception.ResponseBody, Is.EqualTo("not-json"));
        });
    }

    [Test]
    public void Send_PropagatesCancellationToHandler()
    {
        var handler = new TestHttpMessageHandler(async (_, cancellationToken) =>
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            throw new InvalidOperationException("The delay should have been cancelled.");
        });
        using var httpClient         = new HttpClient(handler);
        using var service            = CreateService(handler, httpClient);
        using var cancellationSource = new CancellationTokenSource();
        cancellationSource.Cancel();

        Assert.That(async () => await service.Send(HttpMethod.Get, "/cancelled", AuthenticationMode.None,
                                                   cancellationToken : cancellationSource.Token),
                    Throws.InstanceOf<OperationCanceledException>());
    }

    [Test]
    public void Send_AppliesSdkTimeoutWithoutChangingHttpClientTimeout()
    {
        var handler = new TestHttpMessageHandler(async (_, cancellationToken) =>
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            throw new InvalidOperationException("The delay should have timed out.");
        });
        using var httpClient = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromMinutes(3)
        };
        using var service = new BangumiHttpService(new BangumiClientOptions
        {
            BaseAddress = new Uri("https://api.example"),
            UserAgent   = "Banned.Bangumi.Test/1.0",
            HttpClient  = httpClient,
            Timeout     = TimeSpan.FromMilliseconds(50)
        });

        Assert.That(async () => await service.Send(HttpMethod.Get, "/timeout", AuthenticationMode.None),
                    Throws.InstanceOf<OperationCanceledException>());
        Assert.That(httpClient.Timeout, Is.EqualTo(TimeSpan.FromMinutes(3)));
    }

    private static TestHttpMessageHandler CreateJsonHandler(
        string         json,
        HttpStatusCode statusCode = HttpStatusCode.OK) =>
        new((_, _) => Task.FromResult(new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        }));

    private static BangumiHttpService CreateService(
        TestHttpMessageHandler handler,
        HttpClient             httpClient,
        string?                accessToken = null)
    {
        Assert.That(httpClient, Is.Not.Null);
        Assert.That(handler, Is.Not.Null);

        return new BangumiHttpService(new BangumiClientOptions
        {
            BaseAddress = new Uri("https://api.example/root"),
            AccessToken = accessToken,
            UserAgent   = "Banned.Bangumi.Test/1.0",
            HttpClient  = httpClient,
            Timeout     = TimeSpan.FromSeconds(5)
        });
    }
}
