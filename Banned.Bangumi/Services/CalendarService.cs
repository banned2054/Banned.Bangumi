using Banned.Bangumi.Exceptions;
using Banned.Bangumi.Models.Calendar;
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

    /// <summary>
    /// 获取每日放送日历。<br/>
    /// Gets the daily airing calendar.
    /// </summary>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>
    /// 按星期分组的每日放送条目。<br/>
    /// The daily airing subjects grouped by weekday.
    /// </returns>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<IReadOnlyList<CalendarDay>> Get(
        CancellationToken cancellationToken = default) =>
        await HttpService.Get<IReadOnlyList<CalendarDay>>(
            "/calendar",
            AuthenticationMode.None,
            cancellationToken).ConfigureAwait(false);
}
