using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

/// <summary>
/// 虚拟角色
/// </summary>
public partial record LegacyCharacter : LegacyMono
{
    [JsonPropertyName("info")]
    public LegacyMonoInfo? Info { get; init; }
    /// <summary>
    /// 声优列表
    /// </summary>
    [JsonPropertyName("actors")]
    public ICollection<LegacyMonoBase>? Actors { get; init; }
}
