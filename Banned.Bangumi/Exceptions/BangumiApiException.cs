using Banned.Bangumi.Models.Common;
using System.Net;

namespace Banned.Bangumi.Exceptions;

/// <summary>
/// 表示 Bangumi API 返回的错误或无效的 API 响应。<br/>
/// Represents an error returned by the Bangumi API or an invalid API response.
/// </summary>
public class BangumiApiException : Exception
{
    internal BangumiApiException(string          message,
                                 HttpStatusCode? statusCode     = null,
                                 ErrorDetail?    error          = null,
                                 string?         responseBody   = null,
                                 Exception?      innerException = null)
        : base(message, innerException)
    {
        StatusCode   = statusCode;
        Error        = error;
        ResponseBody = responseBody;
    }

    /// <summary>
    /// 获取响应的 HTTP 状态码（如果已收到响应）。<br/>
    /// Gets the HTTP status code, when a response was received.
    /// </summary>
    public HttpStatusCode? StatusCode { get; }

    /// <summary>
    /// 获取结构化 API 错误（如果能够读取）。<br/>
    /// Gets the structured API error, when one could be read.
    /// </summary>
    public ErrorDetail? Error { get; }

    /// <summary>
    /// 获取用于诊断且长度受限的响应正文副本。<br/>
    /// Gets a bounded copy of the response body for diagnostics.
    /// </summary>
    public string? ResponseBody { get; }
}
