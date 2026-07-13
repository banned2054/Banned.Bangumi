namespace Banned.Bangumi.Models.Enums;

/// <summary>
/// 指定与条目类型对应的条目分类。<br/>
/// Specifies a subject category associated with a subject type.
/// </summary>
/// <remarks>
/// 数值 <c>1</c>、<c>2</c> 和 <c>3</c> 在动画与三次元条目中具有不同含义，应结合条目类型使用。<br/>
/// Values <c>1</c>, <c>2</c>, and <c>3</c> have different meanings for anime and live-action subjects and must be used with the subject type.
/// </remarks>
public enum SubjectCategory
{
    /// <summary>其他分类。 / Other category.</summary>
    Other = 0,

    /// <summary>电视动画。 / Television anime.</summary>
    AnimeTelevision = 1,

    /// <summary>日剧。 / Japanese drama.</summary>
    JapaneseDrama = 1,

    /// <summary>原创动画录像。 / Original video animation.</summary>
    AnimeOriginalVideoAnimation = 2,

    /// <summary>欧美剧。 / Western drama.</summary>
    WesternDrama = 2,

    /// <summary>动画电影。 / Anime movie.</summary>
    AnimeMovie = 3,

    /// <summary>华语剧。 / Chinese-language drama.</summary>
    ChineseDrama = 3,

    /// <summary>网络动画。 / Web anime.</summary>
    AnimeWeb = 5,

    /// <summary>漫画。 / Comic.</summary>
    BookComic = 1001,

    /// <summary>小说。 / Novel.</summary>
    BookNovel = 1002,

    /// <summary>画集。 / Illustration collection.</summary>
    BookIllustration = 1003,

    /// <summary>游戏。 / Game.</summary>
    Game = 4001,

    /// <summary>软件。 / Software.</summary>
    GameSoftware = 4002,

    /// <summary>游戏扩展包。 / Game expansion.</summary>
    GameExpansion = 4003,

    /// <summary>桌面游戏。 / Tabletop game.</summary>
    GameTabletop = 4005,

    /// <summary>电视剧。 / Television series.</summary>
    LiveActionTelevision = 6001,

    /// <summary>真人电影。 / Live-action movie.</summary>
    LiveActionMovie = 6002,

    /// <summary>演出。 / Live performance.</summary>
    LiveActionPerformance = 6003,

    /// <summary>综艺。 / Variety show.</summary>
    LiveActionVariety = 6004,
}
