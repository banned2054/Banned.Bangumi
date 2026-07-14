using Banned.Bangumi.Models.Common;
using System.Net;

namespace Banned.Bangumi.Exceptions;

/// <summary>
/// 表示 Bangumi API 返回的 HTTP 429 响应。<br/>
/// Represents an HTTP 429 response from the Bangumi API.
/// </summary>
public sealed class BangumiRateLimitException : BangumiApiException
{
    internal BangumiRateLimitException(
        string         message,
        HttpStatusCode statusCode,
        ErrorDetail?   error,
        string?        responseBody,
        TimeSpan?      retryAfter)
        : base(message, statusCode, error, responseBody)
    {
        RetryAfter = retryAfter;
    }

    /// <summary>
    /// 获取服务端建议的重试等待时间（如果可用）。<br/>
    /// Gets the server-recommended delay before retrying, when available.
    /// </summary>
    public TimeSpan? RetryAfter { get; }
}
