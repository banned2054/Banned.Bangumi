using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Calendar;

/// <summary>
/// 表示每日放送使用的星期信息。<br/>
/// Represents weekday information used by the airing calendar.
/// </summary>
public sealed record CalendarWeekday
{
    /// <summary>
    /// 获取星期的英文简称。<br/>
    /// Gets the English abbreviation of the weekday.
    /// </summary>
    [JsonPropertyName("en")]
    public string? English { get; init; }

    /// <summary>
    /// 获取星期的中文名称。<br/>
    /// Gets the Chinese name of the weekday.
    /// </summary>
    [JsonPropertyName("cn")]
    public string? Chinese { get; init; }

    /// <summary>
    /// 获取星期的日文名称。<br/>
    /// Gets the Japanese name of the weekday.
    /// </summary>
    [JsonPropertyName("ja")]
    public string? Japanese { get; init; }

    /// <summary>
    /// 获取星期编号。<br/>
    /// Gets the weekday identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public int? Id { get; init; }
}
