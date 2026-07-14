using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Persons;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Collections;

/// <summary>
/// 表示用户的人物收藏信息。<br/>
/// Represents a user's person collection information.
/// </summary>
public sealed record UserPersonCollection
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

    /// <summary>获取收藏时间。<br/>Gets the time when the person was collected.</summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; init; }
}
