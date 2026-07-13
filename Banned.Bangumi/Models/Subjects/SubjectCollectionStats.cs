using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Subjects;

/// <summary>
/// 表示条目各收藏状态的人数统计。<br/>
/// Represents subject collection counts by collection status.
/// </summary>
public sealed record SubjectCollectionStats
{
    /// <summary>获取想看、想读或想玩的人数。<br/>Gets the number of users wishing to consume the subject.</summary>
    [JsonPropertyName("wish")]
    public int? Wish { get; init; }

    /// <summary>获取看过、读过或玩过的人数。<br/>Gets the number of users who completed the subject.</summary>
    [JsonPropertyName("collect")]
    public int? Collect { get; init; }

    /// <summary>获取在看、在读或在玩的人数。<br/>Gets the number of users currently consuming the subject.</summary>
    [JsonPropertyName("doing")]
    public int? Doing { get; init; }

    /// <summary>获取搁置人数。<br/>Gets the number of users who put the subject on hold.</summary>
    [JsonPropertyName("on_hold")]
    public int? OnHold { get; init; }

    /// <summary>获取抛弃人数。<br/>Gets the number of users who dropped the subject.</summary>
    [JsonPropertyName("dropped")]
    public int? Dropped { get; init; }
}
