using Banned.Bangumi.Models.Enums;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

/// <summary>
/// 用户信息
/// </summary>
public partial record LegacyUser
{
    /// <summary>
    /// 用户 id
    /// </summary>
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    /// <summary>
    /// 用户主页地址
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }

    [JsonPropertyName("username")]
    public string? Username { get; init; }

    /// <summary>
    /// 昵称
    /// </summary>
    [JsonPropertyName("nickname")]
    public string? Nickname { get; init; }

    /// <summary>
    /// 头像地址
    /// </summary>
    [JsonPropertyName("avatar")]
    public Avatar? Avatar { get; init; }

    /// <summary>
    /// 签名
    /// </summary>
    [JsonPropertyName("sign")]
    public string? Sign { get; init; }

    [JsonPropertyName("usergroup")]
    public LegacyUserGroup? UserGroup { get; init; }
}