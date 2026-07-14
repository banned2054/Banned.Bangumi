using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Common;

/// <summary>
/// 表示内容创建者的公开用户信息。<br/>
/// Represents public user information for a content creator.
/// </summary>
public sealed record Creator
{
    /// <summary>获取用户名。<br/>Gets the username.</summary>
    [JsonPropertyName("username")]
    public string Username { get; init; } = string.Empty;

    /// <summary>获取昵称。<br/>Gets the nickname.</summary>
    [JsonPropertyName("nickname")]
    public string Nickname { get; init; } = string.Empty;
}
