namespace Banned.Bangumi.Models.Collections;

/// <summary>
/// 指定条目收藏状态。<br/>
/// Specifies a subject collection status.
/// </summary>
public enum SubjectCollectionType
{
    /// <summary>想看。 / Wish to watch, read, or play.</summary>
    Wish = 1,

    /// <summary>看过。 / Completed.</summary>
    Done = 2,

    /// <summary>在看。 / In progress.</summary>
    Doing = 3,

    /// <summary>搁置。 / On hold.</summary>
    OnHold = 4,

    /// <summary>抛弃。 / Dropped.</summary>
    Dropped = 5,
}
