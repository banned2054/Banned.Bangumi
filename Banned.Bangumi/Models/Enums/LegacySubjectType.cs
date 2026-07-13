namespace Banned.Bangumi.Models.Enums;

/// <summary>
/// 表示 Legacy API 使用的条目类型。<br/>
/// Represents a subject type used by the Legacy API.
/// </summary>
public enum LegacySubjectType
{
    /// <summary>书籍。 / Book.</summary>
    Book = 1,

    /// <summary>动画。 / Anime.</summary>
    Anime = 2,

    /// <summary>音乐。 / Music.</summary>
    Music = 3,

    /// <summary>游戏。 / Game.</summary>
    Game = 4,

    /// <summary>三次元。 / Real-world subject.</summary>
    Real = 6
}
