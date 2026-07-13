using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

/// <summary>
/// 日志
/// </summary>
public partial record LegacyBlog
{
    /// <summary>
    /// ID
    /// </summary>
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    /// <summary>
    /// 地址
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }

    /// <summary>
    /// 标题
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    /// <summary>
    /// 概览
    /// </summary>
    [JsonPropertyName("summary")]
    public string? Summary { get; init; }

    /// <summary>
    /// 图片
    /// </summary>
    [JsonPropertyName("image")]
    public string? Image { get; init; }

    [JsonPropertyName("replies")]
    public int? Replies { get; init; }

    /// <summary>
    /// 发布时间
    /// </summary>
    [JsonPropertyName("timestamp")]
    public int? Timestamp { get; init; }

    /// <summary>
    /// 发布时间
    /// </summary>
    [JsonPropertyName("dateline")]
    public string? Dateline { get; init; }

    [JsonPropertyName("user")]
    public LegacyUser? User { get; init; }
}