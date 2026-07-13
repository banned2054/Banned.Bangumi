using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Subjects;

/// <summary>
/// 表示条目标签及其使用次数。<br/>
/// Represents a subject tag and its usage count.
/// </summary>
public sealed record SubjectTag
{
    /// <summary>获取标签名称。<br/>Gets the tag name.</summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>获取标签使用次数。<br/>Gets the tag usage count.</summary>
    [JsonPropertyName("count")]
    public int Count { get; init; }
}
