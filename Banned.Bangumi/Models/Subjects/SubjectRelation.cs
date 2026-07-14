using Banned.Bangumi.Models.Common;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Subjects;

/// <summary>
/// 表示两个条目之间的关联。<br/>
/// Represents a relationship between two subjects.
/// </summary>
public sealed record SubjectRelation
{
    /// <summary>获取关联条目 ID。<br/>Gets the related subject ID.</summary>
    [JsonPropertyName("id")]
    public int Id { get; init; }

    /// <summary>获取 API 返回的关联条目类型值。<br/>Gets the related subject type value returned by the API.</summary>
    [JsonPropertyName("type")]
    public int Type { get; init; }

    /// <summary>获取关联条目的原始名称。<br/>Gets the related subject's original name.</summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>获取关联条目的中文名称。<br/>Gets the related subject's Chinese name.</summary>
    [JsonPropertyName("name_cn")]
    public string NameCn { get; init; } = string.Empty;

    /// <summary>获取关联条目的图片。<br/>Gets images for the related subject.</summary>
    [JsonPropertyName("images")]
    public Images? Images { get; init; }

    /// <summary>获取两个条目之间的关系。<br/>Gets the relationship between the subjects.</summary>
    [JsonPropertyName("relation")]
    public string Relation { get; init; } = string.Empty;
}
