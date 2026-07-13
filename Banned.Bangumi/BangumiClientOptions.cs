namespace Banned.Bangumi;

/// <summary>
/// 配置 <see cref="BangumiClient"/> 实例。<br/>
/// Configures a <see cref="BangumiClient"/> instance.
/// </summary>
public sealed class BangumiClientOptions
{
    /// <summary>
    /// 获取 API 请求使用的基础地址。<br/>
    /// Gets the base address used for API requests.
    /// </summary>
    public Uri BaseAddress { get; init; } = new("https://api.bgm.tv");

    /// <summary>
    /// 获取可选的 Bangumi Access Token。<br/>
    /// Gets the optional Bangumi access token.
    /// </summary>
    public string? AccessToken { get; init; }

    /// <summary>
    /// 获取每个请求发送的 User-Agent。<br/>
    /// Gets the User-Agent sent with every request.
    /// </summary>
    public required string UserAgent { get; init; }

    /// <summary>
    /// 获取可选的、由调用方持有的 HTTP 客户端。<br/>
    /// Gets an optional caller-owned HTTP client.
    /// </summary>
    /// <remarks>
    /// SDK 永远不会释放或重新配置调用方传入的客户端。<br/>
    /// A supplied client is never disposed or reconfigured by the SDK.
    /// </remarks>
    public HttpClient? HttpClient { get; init; }

    /// <summary>
    /// 获取 SDK 应用于每个请求的超时时间。<br/>
    /// Gets the timeout applied by the SDK to each request.
    /// </summary>
    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(30);
}
