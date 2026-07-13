using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Subjects;

/// <summary>
/// 表示 Legacy API 返回的条目评分信息。<br/>
/// Represents subject rating information returned by the Legacy API.
/// </summary>
public sealed record LegacySubjectRating
{
    /// <summary>获取总评分人数。<br/>Gets the total number of ratings.</summary>
    [JsonPropertyName("total")]
    public int? Total { get; init; }

    /// <summary>获取各分值的评分人数。<br/>Gets the rating count for each score.</summary>
    [JsonPropertyName("count")]
    public SubjectRatingDistribution? Count { get; init; }

    /// <summary>获取平均评分。<br/>Gets the average score.</summary>
    [JsonPropertyName("score")]
    public double? Score { get; init; }
}
