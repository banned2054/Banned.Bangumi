using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Indices;

/// <summary>
/// 表示目录条目修改请求。<br/>
/// Represents a request to update a subject in an index.
/// </summary>
public sealed record IndexSubjectUpdateRequest
{
    /// <summary>获取或初始化排序值；值越小越靠前。<br/>Gets or initializes the sort value; lower values appear first.</summary>
    [JsonPropertyName("sort")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Sort { get; init; }

    /// <summary>获取或初始化目录条目备注。<br/>Gets or initializes the index subject comment.</summary>
    [JsonPropertyName("comment")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Comment { get; init; }
}
