using Banned.Bangumi.Models.Subjects;

namespace Banned.Bangumi.Models.Collections;

/// <summary>
/// 表示浏览用户条目收藏时使用的查询条件。<br/>
/// Represents query criteria used to browse a user's subject collections.
/// </summary>
public sealed record SubjectCollectionBrowseRequest
{
    /// <summary>获取或初始化条目类型筛选值。<br/>Gets or initializes the subject type filter.</summary>
    public SubjectType? SubjectType { get; init; }

    /// <summary>获取或初始化收藏状态筛选值。<br/>Gets or initializes the collection status filter.</summary>
    public SubjectCollectionType? Type { get; init; }

    /// <summary>获取或初始化返回数量，允许范围为 1 到 50。<br/>Gets or initializes the result limit, from 1 through 50.</summary>
    public int? Limit { get; init; }

    /// <summary>获取或初始化分页偏移量。<br/>Gets or initializes the pagination offset.</summary>
    public int? Offset { get; init; }
}
