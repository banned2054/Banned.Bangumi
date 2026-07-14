using Banned.Bangumi.Models.Episodes;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

/// <summary>
/// 章节信息
/// </summary>
public partial record LegacyEpisode
{
    /// <summary>
    /// 章节 ID
    /// </summary>
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    /// <summary>
    /// 章节地址
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }

    [JsonPropertyName("type")]
    public EpisodeType? Type { get; init; }

    /// <summary>
    /// 集数
    /// </summary>
    [JsonPropertyName("sort")]
    public int? Sort { get; init; }

    /// <summary>
    /// 标题
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("name_cn")]
    public string? NameCn { get; init; }

    /// <summary>
    /// 时长
    /// </summary>
    [JsonPropertyName("duration")]
    public string? Duration { get; init; }

    [JsonPropertyName("airdate")]
    public string? Airdate { get; init; }

    /// <summary>
    /// 回复数量
    /// </summary>
    [JsonPropertyName("comment")]
    public int? Comment { get; init; }

    [JsonPropertyName("desc")]
    public string? Desc { get; init; }

    [JsonPropertyName("status")]
    [JsonConverter(typeof(JsonStringEnumConverter<LegacyEpisodeStatus>))]
    public LegacyEpisodeStatus? Status { get; init; }
}
