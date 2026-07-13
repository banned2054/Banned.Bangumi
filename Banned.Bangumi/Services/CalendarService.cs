using Banned.Bangumi.Services.Internal;

namespace Banned.Bangumi.Services;

/// <summary>
/// 提供 Bangumi 每日放送操作。<br/>
/// Provides operations for the Bangumi calendar.
/// </summary>
public sealed class CalendarService
{
    internal CalendarService(BangumiHttpService httpService) => HttpService = httpService;

    internal BangumiHttpService HttpService { get; }
}
