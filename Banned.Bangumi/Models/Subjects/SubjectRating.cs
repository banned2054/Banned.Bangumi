using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Subjects;

/// <summary>
/// 表示条目评分信息。<br/>
/// Represents subject rating information.
/// </summary>
public sealed record SubjectRating
{
    /// <summary>获取条目排名。<br/>Gets the subject rank.</summary>
    [JsonPropertyName("rank")]
    public int Rank { get; init; }

    /// <summary>获取参与评分的人数。<br/>Gets the total number of ratings.</summary>
    [JsonPropertyName("total")]
    public int Total { get; init; }

    /// <summary>获取各分值的评分人数。<br/>Gets rating counts by score.</summary>
    [JsonPropertyName("count")]
    public SubjectRatingDistribution Count { get; init; } = new();

    /// <summary>获取平均评分。<br/>Gets the average score.</summary>
    [JsonPropertyName("score")]
    public double Score { get; init; }
}
