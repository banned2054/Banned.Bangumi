using Banned.Bangumi.Models.Common;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Users;

/// <summary>
/// 表示用户的公开资料。<br/>
/// Represents a user's public profile.
/// </summary>
public record User
{
    /// <summary>获取用户 ID。<br/>Gets the user ID.</summary>
    [JsonPropertyName("id")]
    public int Id { get; init; }

    /// <summary>获取唯一用户名。<br/>Gets the unique username.</summary>
    [JsonPropertyName("username")]
    public string Username { get; init; } = string.Empty;

    /// <summary>获取用户昵称。<br/>Gets the user's nickname.</summary>
    [JsonPropertyName("nickname")]
    public string Nickname { get; init; } = string.Empty;

    /// <summary>获取用户组。<br/>Gets the user group.</summary>
    [JsonPropertyName("user_group")]
    public UserGroup UserGroup { get; init; }

    /// <summary>获取用户头像。<br/>Gets the user's avatar images.</summary>
    [JsonPropertyName("avatar")]
    public Images Avatar { get; init; } = new();

    /// <summary>获取个人签名。<br/>Gets the personal signature.</summary>
    [JsonPropertyName("sign")]
    public string Sign { get; init; } = string.Empty;
}
