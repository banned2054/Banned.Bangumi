using Banned.Bangumi.Models.Characters;
using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Subjects;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Persons;

/// <summary>
/// 表示与人物关联的角色及其配套条目信息。<br/>
/// Represents a character related to a person together with associated subject information.
/// </summary>
public sealed record PersonRelatedCharacter
{
    /// <summary>获取角色 ID。<br/>Gets the character ID.</summary>
    [JsonPropertyName("id")]
    public int Id { get; init; }

    /// <summary>获取角色名称。<br/>Gets the character name.</summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>获取角色资源类型。<br/>Gets the character resource type.</summary>
    [JsonPropertyName("type")]
    public CharacterType Type { get; init; }

    /// <summary>获取角色图片；无图片时为 <see langword="null"/>。<br/>Gets character images, or <see langword="null"/> when unavailable.</summary>
    [JsonPropertyName("images")]
    public Images? Images { get; init; }

    /// <summary>获取关联条目 ID。<br/>Gets the related subject ID.</summary>
    [JsonPropertyName("subject_id")]
    public int SubjectId { get; init; }

    /// <summary>获取关联条目类型。<br/>Gets the related subject type.</summary>
    [JsonPropertyName("subject_type")]
    public SubjectType SubjectType { get; init; }

    /// <summary>获取关联条目的原始名称。<br/>Gets the original name of the related subject.</summary>
    [JsonPropertyName("subject_name")]
    public string SubjectName { get; init; } = string.Empty;

    /// <summary>获取关联条目的中文名称。<br/>Gets the Chinese name of the related subject.</summary>
    [JsonPropertyName("subject_name_cn")]
    public string SubjectNameCn { get; init; } = string.Empty;

    /// <summary>获取人物与角色的职务说明。<br/>Gets the person's staff role for the character.</summary>
    [JsonPropertyName("staff")]
    public string? Staff { get; init; }
}
