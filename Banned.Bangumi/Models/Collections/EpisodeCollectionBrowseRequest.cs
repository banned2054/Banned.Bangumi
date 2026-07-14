using Banned.Bangumi.Models.Episodes;

namespace Banned.Bangumi.Models.Collections;

/// <summary>
/// 表示浏览当前用户章节收藏时使用的查询条件。<br/>
/// Represents query criteria used to browse the current user's episode collections.
/// </summary>
public sealed record EpisodeCollectionBrowseRequest
{
    /// <summary>获取或初始化章节类型筛选值。<br/>Gets or initializes the episode type filter.</summary>
    public EpisodeType? EpisodeType { get; init; }

    /// <summary>获取或初始化返回数量，允许范围为 1 到 1000。<br/>Gets or initializes the result limit, from 1 through 1000.</summary>
    public int? Limit { get; init; }

    /// <summary>获取或初始化分页偏移量。<br/>Gets or initializes the pagination offset.</summary>
    public int? Offset { get; init; }
}
