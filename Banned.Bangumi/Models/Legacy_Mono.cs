using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

/// <summary>
/// 人物
/// </summary>
public partial record LegacyMono : LegacyMonoBase
{
    /// <summary>
    /// 简体中文名
    /// </summary>
    [JsonPropertyName("name_cn")]
    public string? NameCn { get; init; }
    /// <summary>
    /// 回复数量
    /// </summary>
    [JsonPropertyName("comment")]
    public int? Comment { get; init; }
    /// <summary>
    /// 收藏人数
    /// </summary>
    [JsonPropertyName("collects")]
    public int? Collects { get; init; }
}
