using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

/// <summary>
/// 表示 Bangumi API 返回的结构化错误详情。<br/>
/// Represents structured error details returned by the Bangumi API.
/// </summary>
public partial record ErrorDetail
{
    /// <summary>
    /// 获取错误标题。<br/>
    /// Gets the error title.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    /// <summary>
    /// 获取错误描述。<br/>
    /// Gets the error description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// 获取错误的补充详情。<br/>
    /// Gets additional error details.
    /// </summary>
    [JsonPropertyName("details")]
    public string? Details { get; init; }
}
