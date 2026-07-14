using Banned.Bangumi.Models.Characters;
using Banned.Bangumi.Models.Common;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Collections;

/// <summary>
/// 表示用户的角色收藏信息。<br/>
/// Represents a user's character collection information.
/// </summary>
public sealed record UserCharacterCollection
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

    /// <summary>获取收藏时间。<br/>Gets the time when the character was collected.</summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; init; }
}
