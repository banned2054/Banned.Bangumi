using Banned.Bangumi.Models.Common;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Persons;

/// <summary>
/// 表示人物概要信息。<br/>
/// Represents summary information about a person.
/// </summary>
public sealed record Person
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

    /// <summary>获取人物简介。<br/>Gets the short summary.</summary>
    [JsonPropertyName("short_summary")]
    public string ShortSummary { get; init; } = string.Empty;

    /// <summary>获取人物是否被锁定。<br/>Gets whether the person is locked.</summary>
    [JsonPropertyName("locked")]
    public bool Locked { get; init; }
}
