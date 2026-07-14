using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Common;

/// <summary>
/// 表示资源的评论和收藏统计。<br/>
/// Represents comment and collection statistics for a resource.
/// </summary>
public sealed record ResourceStatistics
{
    /// <summary>获取评论数。<br/>Gets the comment count.</summary>
    [JsonPropertyName("comments")]
    public int CommentCount { get; init; }

    /// <summary>获取收藏数。<br/>Gets the collection count.</summary>
    [JsonPropertyName("collects")]
    public int CollectionCount { get; init; }
}
