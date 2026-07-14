using Banned.Bangumi.Models.Enums;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Characters;

/// <summary>
/// 表示与角色关联的条目。<br/>
/// Represents a subject related to a character.
/// </summary>
public sealed record RelatedSubject
{
    /// <summary>获取条目 ID。<br/>Gets the subject ID.</summary>
    [JsonPropertyName("id")]
    public int Id { get; init; }

    /// <summary>获取条目类型。<br/>Gets the subject type.</summary>
    [JsonPropertyName("type")]
    public SubjectType Type { get; init; }

    /// <summary>获取关联职务说明。<br/>Gets the related staff role.</summary>
    [JsonPropertyName("staff")]
    public string Staff { get; init; } = string.Empty;

    /// <summary>获取参与的章节或曲目。<br/>Gets the involved episodes or tracks.</summary>
    [JsonPropertyName("eps")]
    public string Episodes { get; init; } = string.Empty;

    /// <summary>获取条目的原始名称。<br/>Gets the original subject name.</summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>获取条目的中文名称。<br/>Gets the Chinese subject name.</summary>
    [JsonPropertyName("name_cn")]
    public string NameCn { get; init; } = string.Empty;

    /// <summary>获取条目图片地址。<br/>Gets the subject image URL.</summary>
    [JsonPropertyName("image")]
    public string? Image { get; init; }
}
