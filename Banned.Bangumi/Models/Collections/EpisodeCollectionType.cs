namespace Banned.Bangumi.Models.Collections;

/// <summary>
/// 指定章节收藏状态。<br/>
/// Specifies an episode collection status.
/// </summary>
public enum EpisodeCollectionType
{
    /// <summary>想看。 / Wish to watch.</summary>
    Wish = 1,

    /// <summary>看过。 / Watched.</summary>
    Done = 2,

    /// <summary>抛弃。 / Dropped.</summary>
    Dropped = 3,
}
