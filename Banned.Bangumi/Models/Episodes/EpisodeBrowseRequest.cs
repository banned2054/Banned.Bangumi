namespace Banned.Bangumi.Models.Episodes;

/// <summary>
/// 表示浏览章节时使用的查询条件。<br/>
/// Represents query criteria used to browse episodes.
/// </summary>
public sealed record EpisodeBrowseRequest
{
    /// <summary>获取或初始化条目 ID。<br/>Gets or initializes the subject ID.</summary>
    public int SubjectId { get; init; }

    /// <summary>获取或初始化章节类型筛选值。<br/>Gets or initializes the episode type filter.</summary>
    public EpisodeType? Type { get; init; }

    /// <summary>获取或初始化返回数量，允许范围为 1 到 200。<br/>Gets or initializes the result limit, from 1 through 200.</summary>
    public int? Limit { get; init; }

    /// <summary>获取或初始化分页偏移量。<br/>Gets or initializes the pagination offset.</summary>
    public int? Offset { get; init; }
}
