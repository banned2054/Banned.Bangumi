using Banned.Bangumi.Models.Users;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record User
{
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    [JsonPropertyName("username")]
    public string? Username { get; init; }

    [JsonPropertyName("nickname")]
    public string? Nickname { get; init; }

    [JsonPropertyName("user_group")]
    public UserGroup? UserGroup { get; init; }

    [JsonPropertyName("avatar")]
    public Avatar? Avatar { get; init; }

    /// <summary>
    /// 个人签名
    /// </summary>
    [JsonPropertyName("sign")]
    public string? Sign { get; init; }
}
