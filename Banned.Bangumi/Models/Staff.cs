using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record Staff : LegacyPerson
{
    /// <summary>
    /// 人物类型
    /// </summary>
    [JsonPropertyName("role_name")]
    public string? RoleName { get; init; }

    /// <summary>
    /// 职位
    /// </summary>
    [JsonPropertyName("jobs")]
    public ICollection<string>? Jobs { get; init; }
}