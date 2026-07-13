namespace Banned.Bangumi.Models.Enums;

/// <summary>
/// 指定条目搜索结果的排序方式。<br/>
/// Specifies the sort order for subject search results.
/// </summary>
public enum SubjectSearchSort
{
    /// <summary>按匹配程度排序。 / Sorts by relevance.</summary>
    Match,

    /// <summary>按收藏热度排序。 / Sorts by collection popularity.</summary>
    Heat,

    /// <summary>按排名排序。 / Sorts by rank.</summary>
    Rank,

    /// <summary>按评分排序。 / Sorts by score.</summary>
    Score,
}
