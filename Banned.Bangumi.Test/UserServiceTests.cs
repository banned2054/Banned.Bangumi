using Banned.Bangumi.Exceptions;
using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Users;
using System.Net;
using System.Text;

namespace Banned.Bangumi.Test;

[TestFixture]
public sealed class UserServiceTests
{
    [Test]
    public async Task Get_SendsEscapedPathWithoutAuthenticationAndDeserializesProfile()
    {
        var handler = CreateJsonHandler("""
                                        {"id":1,"username":"sai/name","nickname":"Sai","user_group":10,"avatar":{"large":"large.jpg","medium":"medium.jpg","small":"small.jpg"},"sign":"Hello"}
                                        """);
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "must-not-be-sent");

        var user = await client.Users.Get("sai/name");

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(request!.Method, Is.EqualTo(HttpMethod.Get));
            Assert.That(request.Uri.AbsoluteUri,
                        Is.EqualTo("https://api.example/root/v0/users/sai%2Fname"));
            Assert.That(request.Authorization, Is.Null);
            Assert.That(request.Body, Is.Null);
            Assert.That(user.Id, Is.EqualTo(1));
            Assert.That(user.UserGroup, Is.EqualTo(UserGroup.User));
            Assert.That(user.Avatar.Small, Is.EqualTo("small.jpg"));
        });
    }

    [Test]
    public async Task GetAvatarUri_SendsExpectedQueryWithoutAuthenticationAndReturnsLocation()
    {
        var avatarUri = new Uri("https://lain.example/pic/user/l/1.jpg");
        var handler = new TestHttpMessageHandler((_, _) =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.Found);
            response.Headers.Location = avatarUri;
            return Task.FromResult(response);
        });
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "must-not-be-sent");

        var result = await client.Users.GetAvatarUri("sai", ImageSize.Large);

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(avatarUri));
            Assert.That(request!.Method, Is.EqualTo(HttpMethod.Get));
            Assert.That(request.Uri,
                        Is.EqualTo(new Uri("https://api.example/root/v0/users/sai/avatar?type=large")));
            Assert.That(request.Authorization, Is.Null);
            Assert.That(request.Body, Is.Null);
        });
    }

    [Test]
    public async Task GetCurrent_SendsRequiredAuthenticationAndDeserializesPrivateFields()
    {
        var handler = CreateJsonHandler("""
                                        {"id":1,"username":"sai","nickname":"Sai","user_group":1,"avatar":{"large":"large.jpg","medium":"medium.jpg","small":"small.jpg"},"sign":"Hello","email":"sai@example.test","reg_time":"2017-12-03T08:51:16+08:00","time_offset":8}
                                        """);
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "required-token");

        var user = await client.Users.GetCurrent();

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(request!.Method, Is.EqualTo(HttpMethod.Get));
            Assert.That(request.Uri, Is.EqualTo(new Uri("https://api.example/root/v0/me")));
            Assert.That(request.Authorization, Is.EqualTo("Bearer required-token"));
            Assert.That(request.Body, Is.Null);
            Assert.That(user.Email, Is.EqualTo("sai@example.test"));
            Assert.That(user.RegistrationTime.Offset, Is.EqualTo(TimeSpan.FromHours(8)));
            Assert.That(user.TimeOffset, Is.EqualTo(8));
        });
    }

    [Test]
    public void GetCurrent_RequiresAccessTokenBeforeSendingRequest()
    {
        var       handler    = CreateJsonHandler("{}");
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient);

        Assert.That(async () => await client.Users.GetCurrent(),
                    Throws.TypeOf<BangumiAuthenticationException>());
        Assert.That(handler.Requests, Is.Empty);
    }

    [Test]
    public void Get_MapsNotFoundResponseToApiException()
    {
        var handler = CreateJsonHandler(
                                        "{\"title\":\"Not Found\",\"description\":\"User does not exist\"}",
                                        HttpStatusCode.NotFound);
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient);

        var exception = Assert.ThrowsAsync<BangumiApiException>(async () => await client.Users.Get("missing"));

        Assert.Multiple(() =>
        {
            Assert.That(exception!.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("User does not exist"));
        });
    }

    [Test]
    public void Get_PropagatesCancellationToken()
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

        Assert.That(async () => await client.Users.Get("sai", cancellationSource.Token),
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
            Assert.That(async () => await client.Users.Get(null!), Throws.TypeOf<ArgumentNullException>());
            Assert.That(async () => await client.Users.Get(" "), Throws.TypeOf<ArgumentException>());
            Assert.That(async () => await client.Users.GetAvatarUri(null!, ImageSize.Small),
                        Throws.TypeOf<ArgumentNullException>());
            Assert.That(async () => await client.Users.GetAvatarUri(" ", ImageSize.Small),
                        Throws.TypeOf<ArgumentException>());
            Assert.That(async () => await client.Users.GetAvatarUri("sai", ImageSize.Grid),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Users.GetAvatarUri("sai", ImageSize.Common),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Users.GetAvatarUri("sai", (ImageSize)999),
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
