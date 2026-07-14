using Banned.Bangumi.Models.Common;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Subjects;

/// <summary>
/// 表示 Legacy API 返回的条目摘要。<br/>
/// Represents a subject summary returned by the Legacy API.
/// </summary>
public record LegacySubjectSummary
{
    /// <summary>
    /// 获取条目 ID。<br/>
    /// Gets the subject ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    /// <summary>
    /// 获取条目页面地址。<br/>
    /// Gets the subject page URL.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }

    /// <summary>
    /// 获取条目类型。<br/>
    /// Gets the subject type.
    /// </summary>
    [JsonPropertyName("type")]
    public SubjectType? Type { get; init; }

    /// <summary>
    /// 获取条目原名。<br/>
    /// Gets the original subject name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// 获取条目中文名。<br/>
    /// Gets the Chinese subject name.
    /// </summary>
    [JsonPropertyName("name_cn")]
    public string? NameCn { get; init; }

    /// <summary>
    /// 获取剧情简介。<br/>
    /// Gets the subject summary.
    /// </summary>
    [JsonPropertyName("summary")]
    public string? Summary { get; init; }

    /// <summary>
    /// 获取放送开始日期。<br/>
    /// Gets the airing start date.
    /// </summary>
    [JsonPropertyName("air_date")]
    public string? AirDate { get; init; }

    /// <summary>
    /// 获取放送星期编号。<br/>
    /// Gets the airing weekday identifier.
    /// </summary>
    [JsonPropertyName("air_weekday")]
    public int? AirWeekday { get; init; }

    /// <summary>
    /// 获取条目封面。<br/>
    /// Gets the subject cover images.
    /// </summary>
    [JsonPropertyName("images")]
    public Images? Images { get; init; }

    /// <summary>
    /// 获取话数。<br/>
    /// Gets the episode count.
    /// </summary>
    [JsonPropertyName("eps")]
    public int? Eps { get; init; }

    /// <summary>
    /// 获取 API 返回的另一话数字段。<br/>
    /// Gets the alternative episode count returned by the API.
    /// </summary>
    [JsonPropertyName("eps_count")]
    public int? EpsCount { get; init; }

    /// <summary>
    /// 获取条目评分信息。<br/>
    /// Gets the subject rating information.
    /// </summary>
    [JsonPropertyName("rating")]
    public LegacySubjectRating? Rating { get; init; }

    /// <summary>
    /// 获取条目排名。<br/>
    /// Gets the subject rank.
    /// </summary>
    [JsonPropertyName("rank")]
    public int? Rank { get; init; }

    /// <summary>
    /// 获取条目收藏统计。<br/>
    /// Gets the subject collection statistics.
    /// </summary>
    [JsonPropertyName("collection")]
    public SubjectCollectionStats? Collection { get; init; }
}
