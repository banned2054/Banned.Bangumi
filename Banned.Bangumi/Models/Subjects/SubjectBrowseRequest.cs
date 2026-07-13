using Banned.Bangumi.Models.Enums;

namespace Banned.Bangumi.Models.Subjects;

/// <summary>
/// 表示浏览条目时使用的查询条件。<br/>
/// Represents query criteria used to browse subjects.
/// </summary>
public sealed record SubjectBrowseRequest
{
    /// <summary>获取或初始化条目类型。<br/>Gets or initializes the subject type.</summary>
    public SubjectType Type { get; init; }

    /// <summary>获取或初始化条目分类。<br/>Gets or initializes the subject category.</summary>
    public SubjectCategory? Category { get; init; }

    /// <summary>获取或初始化是否仅返回书籍系列主条目。<br/>Gets or initializes whether only main book-series subjects are returned.</summary>
    public bool? Series { get; init; }

    /// <summary>获取或初始化游戏平台。<br/>Gets or initializes the game platform.</summary>
    public string? Platform { get; init; }

    /// <summary>获取或初始化排序方式。<br/>Gets or initializes the sort order.</summary>
    public SubjectBrowseSort? Sort { get; init; }

    /// <summary>获取或初始化年份筛选值。<br/>Gets or initializes the year filter.</summary>
    public int? Year { get; init; }

    /// <summary>获取或初始化月份筛选值。<br/>Gets or initializes the month filter.</summary>
    public int? Month { get; init; }

    /// <summary>获取或初始化返回数量，允许范围为 1 到 50。<br/>Gets or initializes the result limit, from 1 through 50.</summary>
    public int? Limit { get; init; }

    /// <summary>获取或初始化分页偏移量。<br/>Gets or initializes the pagination offset.</summary>
    public int? Offset { get; init; }
}
