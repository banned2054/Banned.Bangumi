using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record LegacyTopic
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

    [JsonPropertyName("main_id")]
    public int? MainId { get; init; }

    /// <summary>
    /// 发布时间
    /// </summary>
    [JsonPropertyName("timestamp")]
    public int? Timestamp { get; init; }

    [JsonPropertyName("lastpost")]
    public int? LastPost { get; init; }

    [JsonPropertyName("replies")]
    public int? Replies { get; init; }

    [JsonPropertyName("user")]
    public LegacyUser? User { get; init; }
}