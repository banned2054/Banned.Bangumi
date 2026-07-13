using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Subjects;

/// <summary>
/// 表示条目各分值的评分人数分布。<br/>
/// Represents the distribution of subject rating counts by score.
/// </summary>
public sealed record SubjectRatingDistribution
{
    /// <summary>获取 1 分的评分人数。<br/>Gets the number of 1-point ratings.</summary>
    [JsonPropertyName("1")]
    public int Score1 { get; init; }

    /// <summary>获取 2 分的评分人数。<br/>Gets the number of 2-point ratings.</summary>
    [JsonPropertyName("2")]
    public int Score2 { get; init; }

    /// <summary>获取 3 分的评分人数。<br/>Gets the number of 3-point ratings.</summary>
    [JsonPropertyName("3")]
    public int Score3 { get; init; }

    /// <summary>获取 4 分的评分人数。<br/>Gets the number of 4-point ratings.</summary>
    [JsonPropertyName("4")]
    public int Score4 { get; init; }

    /// <summary>获取 5 分的评分人数。<br/>Gets the number of 5-point ratings.</summary>
    [JsonPropertyName("5")]
    public int Score5 { get; init; }

    /// <summary>获取 6 分的评分人数。<br/>Gets the number of 6-point ratings.</summary>
    [JsonPropertyName("6")]
    public int Score6 { get; init; }

    /// <summary>获取 7 分的评分人数。<br/>Gets the number of 7-point ratings.</summary>
    [JsonPropertyName("7")]
    public int Score7 { get; init; }

    /// <summary>获取 8 分的评分人数。<br/>Gets the number of 8-point ratings.</summary>
    [JsonPropertyName("8")]
    public int Score8 { get; init; }

    /// <summary>获取 9 分的评分人数。<br/>Gets the number of 9-point ratings.</summary>
    [JsonPropertyName("9")]
    public int Score9 { get; init; }

    /// <summary>获取 10 分的评分人数。<br/>Gets the number of 10-point ratings.</summary>
    [JsonPropertyName("10")]
    public int Score10 { get; init; }
}
