using System.Net;
using Banned.Bangumi.Models.Common;

namespace Banned.Bangumi.Exceptions;

/// <summary>
/// 表示缺少 Access Token 或 API 返回的身份验证失败。<br/>
/// Represents a missing access token or an authentication failure from the API.
/// </summary>
public sealed class BangumiAuthenticationException : BangumiApiException
{
    internal BangumiAuthenticationException(
        string message,
        HttpStatusCode? statusCode = null,
        ErrorDetail? error = null,
        string? responseBody = null)
        : base(message, statusCode, error, responseBody)
    {
    }
}
