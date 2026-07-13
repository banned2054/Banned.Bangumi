using System.Text.Json.Serialization;
using Banned.Bangumi.Models.Subjects;

namespace Banned.Bangumi.Models.Calendar;

/// <summary>
/// 表示某个星期日期的每日放送条目分组。<br/>
/// Represents a calendar group of airing subjects for a weekday.
/// </summary>
public sealed record CalendarDay
{
    /// <summary>
    /// 获取星期信息。<br/>
    /// Gets the weekday information.
    /// </summary>
    [JsonPropertyName("weekday")]
    public CalendarWeekday? Weekday { get; init; }

    /// <summary>
    /// 获取该星期日期放送的条目。<br/>
    /// Gets the subjects airing on the weekday.
    /// </summary>
    [JsonPropertyName("items")]
    public IReadOnlyList<LegacySubjectSummary> Items { get; init; } = [];
}
