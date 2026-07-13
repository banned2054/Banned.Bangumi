using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Subjects;

/// <summary>
/// 表示条目不同尺寸的图片地址。<br/>
/// Represents subject image URLs in different sizes.
/// </summary>
public sealed record SubjectImages
{
    /// <summary>获取大尺寸图片地址。<br/>Gets the large image URL.</summary>
    [JsonPropertyName("large")]
    public string Large { get; init; } = string.Empty;

    /// <summary>获取常用尺寸图片地址。<br/>Gets the common-size image URL.</summary>
    [JsonPropertyName("common")]
    public string Common { get; init; } = string.Empty;

    /// <summary>获取中等尺寸图片地址。<br/>Gets the medium image URL.</summary>
    [JsonPropertyName("medium")]
    public string Medium { get; init; } = string.Empty;

    /// <summary>获取小尺寸图片地址。<br/>Gets the small image URL.</summary>
    [JsonPropertyName("small")]
    public string Small { get; init; } = string.Empty;

    /// <summary>获取网格尺寸图片地址。<br/>Gets the grid-size image URL.</summary>
    [JsonPropertyName("grid")]
    public string Grid { get; init; } = string.Empty;
}
