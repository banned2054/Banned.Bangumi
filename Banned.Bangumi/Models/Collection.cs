using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public record Collection
{
    /// <summary>
    /// 想做
    /// </summary>
    [JsonPropertyName("wish")]
    public int? Wish { get; init; }

    /// <summary>
    /// 做过
    /// </summary>
    [JsonPropertyName("collect")]
    public int? Collect { get; init; }

    /// <summary>
    /// 在做
    /// </summary>
    [JsonPropertyName("doing")]
    public int? Doing { get; init; }

    /// <summary>
    /// 搁置
    /// </summary>
    [JsonPropertyName("on_hold")]
    public int? OnHold { get; init; }

    /// <summary>
    /// 抛弃
    /// </summary>
    [JsonPropertyName("dropped")]
    public int? Dropped { get; init; }
}