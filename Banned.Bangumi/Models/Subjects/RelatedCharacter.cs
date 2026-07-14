using Banned.Bangumi.Models.Characters;
using Banned.Bangumi.Models.Persons;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Subjects;

/// <summary>
/// 表示条目中的关联角色。<br/>
/// Represents a character related to a subject.
/// </summary>
public sealed record RelatedCharacter
{
    /// <summary>获取角色 ID。<br/>Gets the character ID.</summary>
    [JsonPropertyName("id")]
    public int Id { get; init; }

    /// <summary>获取角色名称。<br/>Gets the character name.</summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>获取角色简介。<br/>Gets the character summary.</summary>
    [JsonPropertyName("summary")]
    public string Summary { get; init; } = string.Empty;

    /// <summary>获取角色资源类型。<br/>Gets the character resource type.</summary>
    [JsonPropertyName("type")]
    public CharacterType Type { get; init; }

    /// <summary>获取角色图片；无图片时为 <see langword="null"/>。<br/>Gets character images, or <see langword="null"/> when unavailable.</summary>
    [JsonPropertyName("images")]
    public PersonImages? Images { get; init; }

    /// <summary>获取角色与条目的关系。<br/>Gets the character's relationship to the subject.</summary>
    [JsonPropertyName("relation")]
    public string Relation { get; init; } = string.Empty;

    /// <summary>获取演员列表。<br/>Gets the actor list.</summary>
    [JsonPropertyName("actors")]
    public IReadOnlyList<Person> Actors { get; init; } = [];
}
