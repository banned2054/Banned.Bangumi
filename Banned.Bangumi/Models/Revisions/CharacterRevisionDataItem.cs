using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Revisions;

/// <summary>
/// 表示角色修订中的一组数据。<br/>
/// Represents one data entry in a character revision.
/// </summary>
public sealed record CharacterRevisionDataItem
{
    /// <summary>获取角色信息框的 Wiki 源文本。<br/>Gets the wiki source text for the character's infobox.</summary>
    [JsonPropertyName("infobox")]
    public string Infobox { get; init; } = string.Empty;

    /// <summary>获取角色简介。<br/>Gets the character summary.</summary>
    [JsonPropertyName("summary")]
    public string Summary { get; init; } = string.Empty;

    /// <summary>获取角色名称。<br/>Gets the character name.</summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>获取角色修订附加数据。<br/>Gets extra character revision data.</summary>
    [JsonPropertyName("extra")]
    public RevisionExtra Extra { get; init; } = new();
}
