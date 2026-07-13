using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Persons;

/// <summary>
/// 表示人物或角色不同尺寸的图片地址。<br/>
/// Represents person or character image URLs in different sizes.
/// </summary>
public sealed record PersonImages
{
    /// <summary>获取大尺寸图片地址。<br/>Gets the large image URL.</summary>
    [JsonPropertyName("large")]
    public string Large { get; init; } = string.Empty;

    /// <summary>获取中等尺寸图片地址。<br/>Gets the medium image URL.</summary>
    [JsonPropertyName("medium")]
    public string Medium { get; init; } = string.Empty;

    /// <summary>获取小尺寸图片地址。<br/>Gets the small image URL.</summary>
    [JsonPropertyName("small")]
    public string Small { get; init; } = string.Empty;

    /// <summary>获取网格尺寸图片地址。<br/>Gets the grid image URL.</summary>
    [JsonPropertyName("grid")]
    public string Grid { get; init; } = string.Empty;
}
