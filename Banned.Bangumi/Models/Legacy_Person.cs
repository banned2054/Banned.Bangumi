using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

/// <summary>
/// 现实人物
/// </summary>
public partial record LegacyPerson : LegacyMono
{
    [JsonPropertyName("info")]
    public LegacyMonoInfo? Info { get; init; }
}
