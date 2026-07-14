using Banned.Bangumi.Exceptions;
using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Users;
using Banned.Bangumi.Services.Internal;

namespace Banned.Bangumi.Services;

/// <summary>
/// 提供 Bangumi 用户操作。<br/>
/// Provides operations for Bangumi users.
/// </summary>
public sealed class UserService
{
    internal UserService(BangumiHttpService httpService) => HttpService = httpService;

    internal BangumiHttpService HttpService { get; }

    /// <summary>
    /// 获取指定用户的公开资料。<br/>
    /// Gets the public profile of the specified user.
    /// </summary>
    /// <param name="username">用户名。 / Username.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>用户公开资料。<br/>The user's public profile.</returns>
    /// <exception cref="ArgumentException"><paramref name="username"/> 为空。 / <paramref name="username"/> is empty.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="username"/> 为 <see langword="null"/>。 / <paramref name="username"/> is <see langword="null"/>.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<User> Get(string username, CancellationToken cancellationToken = default)
    {
        ValidateUsername(username);
        return await HttpService
                    .Get<User>($"/v0/users/{Uri.EscapeDataString(username)}", AuthenticationMode.None,
                               cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 获取指定尺寸用户头像的重定向目标。<br/>
    /// Gets the redirect target for a user avatar of the specified size.
    /// </summary>
    /// <param name="username">用户名。 / Username.</param>
    /// <param name="size">头像尺寸。 / Avatar size.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>头像地址。<br/>The avatar URI.</returns>
    /// <exception cref="ArgumentException"><paramref name="username"/> 为空。 / <paramref name="username"/> is empty.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="username"/> 为 <see langword="null"/>。 / <paramref name="username"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> 不是头像接口支持的尺寸。 / <paramref name="size"/> is not supported by the avatar endpoint.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或未返回重定向目标。 / The API returns an error or does not return a redirect target.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<Uri> GetAvatarUri(string username, ImageSize size, CancellationToken cancellationToken = default)
    {
        ValidateUsername(username);
        if (size is not (ImageSize.Small or ImageSize.Large or ImageSize.Medium))
        {
            throw new ArgumentOutOfRangeException(nameof(size), size, "The avatar size is invalid.");
        }

        var path = BangumiHttpService.AddQueryString($"/v0/users/{Uri.EscapeDataString(username)}/avatar",
                                                     new Dictionary<string, string?>
                                                         { ["type"] = size.ToString().ToLowerInvariant() });

        return await HttpService.GetRedirectUri(path, AuthenticationMode.None, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 获取当前访问令牌对应的用户。<br/>
    /// Gets the user associated with the current access token.
    /// </summary>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>当前用户资料。<br/>The current user's profile.</returns>
    /// <exception cref="BangumiAuthenticationException">未配置访问令牌或 API 拒绝鉴权。 / No access token is configured or the API rejects authentication.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<CurrentUser> GetCurrent(CancellationToken cancellationToken = default) => await HttpService
       .Get<CurrentUser>("/v0/me", AuthenticationMode.Required, cancellationToken).ConfigureAwait(false);

    private static void ValidateUsername(string username) =>
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
}
