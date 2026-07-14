using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Common;

/// <summary>
/// 表示不同尺寸的图片地址；API 可能只返回其中一部分尺寸。<br/>
/// Represents image URLs in different sizes; the API may return only a subset of the sizes.
/// </summary>
public sealed record Images
{
    /// <summary>获取大尺寸图片地址；未提供时为 <see langword="null"/>。<br/>Gets the large image URL, or <see langword="null"/> when unavailable.</summary>
    [JsonPropertyName("large")]
    public string? Large { get; init; }

    /// <summary>
    /// 获取常用尺寸图片地址；该尺寸用于条目图片，未提供时为 <see langword="null"/>。<br/>
    /// Gets the common-size image URL; this size is used for subject images, or <see langword="null"/> when unavailable.
    /// </summary>
    [JsonPropertyName("common")]
    public string? Common { get; init; }

    /// <summary>获取中等尺寸图片地址；未提供时为 <see langword="null"/>。<br/>Gets the medium image URL, or <see langword="null"/> when unavailable.</summary>
    [JsonPropertyName("medium")]
    public string? Medium { get; init; }

    /// <summary>获取小尺寸图片地址；未提供时为 <see langword="null"/>。<br/>Gets the small image URL, or <see langword="null"/> when unavailable.</summary>
    [JsonPropertyName("small")]
    public string? Small { get; init; }

    /// <summary>
    /// 获取网格尺寸图片地址；用户头像不提供该尺寸，未提供时为 <see langword="null"/>。<br/>
    /// Gets the grid-size image URL; user avatars do not provide this size, or <see langword="null"/> when unavailable.
    /// </summary>
    [JsonPropertyName("grid")]
    public string? Grid { get; init; }
}
