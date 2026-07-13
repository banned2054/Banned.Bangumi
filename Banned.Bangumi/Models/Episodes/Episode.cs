using Banned.Bangumi.Models.Enums;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Episodes;

/// <summary>
/// 表示 Bangumi 章节摘要。<br/>
/// Represents a Bangumi episode summary.
/// </summary>
public sealed record Episode
{
    /// <summary>获取章节 ID。<br/>Gets the episode ID.</summary>
    [JsonPropertyName("id")]
    public int Id { get; init; }

    /// <summary>获取章节类型。<br/>Gets the episode type.</summary>
    [JsonPropertyName("type")]
    public EpisodeType Type { get; init; }

    /// <summary>获取原始名称。<br/>Gets the original name.</summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>获取中文名称。<br/>Gets the Chinese name.</summary>
    [JsonPropertyName("name_cn")]
    public string NameCn { get; init; } = string.Empty;

    /// <summary>获取同类型章节中的排序值。<br/>Gets the sort value among episodes of the same type.</summary>
    [JsonPropertyName("sort")]
    public double Sort { get; init; }

    /// <summary>获取条目内从 1 开始的集数；非本篇章节中该值可能缺失。<br/>Gets the one-based episode number within the subject; this value may be absent for non-main-story episodes.</summary>
    [JsonPropertyName("ep")]
    public double? EpisodeNumber { get; init; }

    /// <summary>获取放送日期。<br/>Gets the air date.</summary>
    [JsonPropertyName("airdate")]
    public string AirDate { get; init; } = string.Empty;

    /// <summary>获取评论数。<br/>Gets the comment count.</summary>
    [JsonPropertyName("comment")]
    public int CommentCount { get; init; }

    /// <summary>获取 Wiki 中填写的原始时长。<br/>Gets the original duration entered in the wiki.</summary>
    [JsonPropertyName("duration")]
    public string Duration { get; init; } = string.Empty;

    /// <summary>获取简介。<br/>Gets the description.</summary>
    [JsonPropertyName("desc")]
    public string Description { get; init; } = string.Empty;

    /// <summary>获取音乐曲目所在的碟片序号。<br/>Gets the disc number containing the music track.</summary>
    [JsonPropertyName("disc")]
    public int Disc { get; init; }

    /// <summary>获取服务端解析的时长（秒）；无法解析或未提供时为 <see langword="null"/>。<br/>Gets the server-parsed duration in seconds, or <see langword="null"/> when unavailable or not provided.</summary>
    [JsonPropertyName("duration_seconds")]
    public int? DurationSeconds { get; init; }
}
