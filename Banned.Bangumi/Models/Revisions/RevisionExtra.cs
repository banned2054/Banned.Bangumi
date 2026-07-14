using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Revisions;

/// <summary>
/// 表示人物或角色修订中的附加数据。<br/>
/// Represents extra data in a person or character revision.
/// </summary>
public sealed record RevisionExtra
{
    /// <summary>获取修订中记录的图片标识。<br/>Gets the image identifier recorded in the revision.</summary>
    [JsonPropertyName("img")]
    public string? Image { get; init; }
}
