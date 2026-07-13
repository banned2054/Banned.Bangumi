namespace Banned.Bangumi.Models.Enums;

/// <summary>
/// 指定章节类型。<br/>
/// Specifies an episode type.
/// </summary>
public enum EpisodeType
{
    /// <summary>本篇。 / Main story.</summary>
    MainStory = 0,

    /// <summary>特别篇。 / Special episode.</summary>
    Special = 1,

    /// <summary>片头。 / Opening sequence.</summary>
    Opening = 2,

    /// <summary>片尾。 / Ending sequence.</summary>
    Ending = 3,

    /// <summary>预告、宣传片或广告。 / Preview, promotional video, or advertisement.</summary>
    PromotionalVideo = 4,

    /// <summary>MAD 视频。 / MAD video.</summary>
    Mad = 5,

    /// <summary>其他类型。 / Other type.</summary>
    Other = 6
}
