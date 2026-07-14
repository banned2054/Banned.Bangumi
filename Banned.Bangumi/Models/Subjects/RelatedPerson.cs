using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Persons;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Subjects;

/// <summary>
/// 表示参与条目制作的人物及其关联信息。<br/>
/// Represents a person involved with a subject and the relationship details.
/// </summary>
public sealed record RelatedPerson
{
    /// <summary>获取人物 ID。<br/>Gets the person ID.</summary>
    [JsonPropertyName("id")]
    public int Id { get; init; }

    /// <summary>获取人物名称。<br/>Gets the person name.</summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>获取人物类型。<br/>Gets the person type.</summary>
    [JsonPropertyName("type")]
    public PersonType Type { get; init; }

    /// <summary>获取人物职业。<br/>Gets the person's careers.</summary>
    [JsonPropertyName("career")]
    public IReadOnlyList<PersonCareer> Careers { get; init; } = [];

    /// <summary>获取人物图片；无图片时为 <see langword="null"/>。<br/>Gets person images, or <see langword="null"/> when unavailable.</summary>
    [JsonPropertyName("images")]
    public Images? Images { get; init; }

    /// <summary>获取人物与条目的关系。<br/>Gets the person's relationship to the subject.</summary>
    [JsonPropertyName("relation")]
    public string Relation { get; init; } = string.Empty;

    /// <summary>获取参与的章节或曲目。<br/>Gets the involved episodes or tracks.</summary>
    [JsonPropertyName("eps")]
    public string Episodes { get; init; } = string.Empty;
}
