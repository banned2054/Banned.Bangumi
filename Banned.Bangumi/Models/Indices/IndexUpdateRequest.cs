using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Indices;

/// <summary>
/// 表示目录信息修改请求。<br/>
/// Represents a request to update index information.
/// </summary>
public sealed record IndexUpdateRequest
{
    /// <summary>获取或初始化目录标题。<br/>Gets or initializes the index title.</summary>
    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Title { get; init; }

    /// <summary>获取或初始化目录描述。<br/>Gets or initializes the index description.</summary>
    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; init; }
}
