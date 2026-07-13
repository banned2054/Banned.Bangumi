using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record Episode
{
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    /// <summary>
    /// `0` 本篇，`1` SP，`2` OP，`3` ED
    /// </summary>
    [JsonPropertyName("type")]
    public int? Type { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("name_cn")]
    public string? NameCn { get; init; }

    /// <summary>
    /// 同类条目的排序和集数
    /// </summary>
    [JsonPropertyName("sort")]
    public double? Sort { get; init; }

    [JsonPropertyName("ep")]
    public double? Ep { get; init; }

    [JsonPropertyName("airdate")]
    public string? Airdate { get; init; }

    [JsonPropertyName("comment")]
    public int? Comment { get; init; }

    /// <summary>
    /// 维基人填写的原始时长
    /// </summary>
    [JsonPropertyName("duration")]
    public string? Duration { get; init; }

    [JsonPropertyName("desc")]
    public string? Desc { get; init; }

    /// <summary>
    /// 音乐曲目的碟片数
    /// </summary>
    [JsonPropertyName("disc")]
    public int? Disc { get; init; }

    [JsonPropertyName("duration_seconds")]
    public int? DurationSeconds { get; init; }
}