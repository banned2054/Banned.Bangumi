using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Revisions;

/// <summary>
/// 表示人物修订中的一组数据。<br/>
/// Represents one data entry in a person revision.
/// </summary>
public sealed record PersonRevisionDataItem
{
    /// <summary>获取人物信息框的 Wiki 源文本。<br/>Gets the wiki source text for the person's infobox.</summary>
    [JsonPropertyName("prsn_infobox")]
    public string Infobox { get; init; } = string.Empty;

    /// <summary>获取人物简介。<br/>Gets the person summary.</summary>
    [JsonPropertyName("prsn_summary")]
    public string Summary { get; init; } = string.Empty;

    /// <summary>获取人物职业标记。<br/>Gets the person's profession flags.</summary>
    [JsonPropertyName("profession")]
    public PersonRevisionProfession Profession { get; init; } = new();

    /// <summary>获取人物修订附加数据。<br/>Gets extra person revision data.</summary>
    [JsonPropertyName("extra")]
    public RevisionExtra Extra { get; init; } = new();

    /// <summary>获取人物名称。<br/>Gets the person name.</summary>
    [JsonPropertyName("prsn_name")]
    public string Name { get; init; } = string.Empty;
}
