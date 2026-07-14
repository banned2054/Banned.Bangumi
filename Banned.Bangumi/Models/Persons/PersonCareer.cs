namespace Banned.Bangumi.Models.Persons;

/// <summary>
/// 指定人物职业。<br/>
/// Specifies a person's career.
/// </summary>
public enum PersonCareer
{
    /// <summary>制作人。 / Producer.</summary>
    Producer = 0,

    /// <summary>漫画家。 / Manga artist.</summary>
    Mangaka = 1,

    /// <summary>艺术家。 / Artist.</summary>
    Artist = 2,

    /// <summary>声优。 / Voice actor.</summary>
    Seiyu = 3,

    /// <summary>作家或编剧。 / Writer.</summary>
    Writer = 4,

    /// <summary>插画家。 / Illustrator.</summary>
    Illustrator = 5,

    /// <summary>演员。 / Actor.</summary>
    Actor = 6,
}
