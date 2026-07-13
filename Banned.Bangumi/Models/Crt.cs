using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record Crt : LegacyCharacter
{
    /// <summary>
    /// 角色类型
    /// </summary>
    [JsonPropertyName("role_name")]
    public string? RoleName { get; init; }
}
