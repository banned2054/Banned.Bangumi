using Banned.Bangumi.Services.Internal;

namespace Banned.Bangumi.Services;

/// <summary>
/// 提供 Bangumi 章节操作。<br/>
/// Provides operations for Bangumi episodes.
/// </summary>
public sealed class EpisodeService
{
    internal EpisodeService(BangumiHttpService httpService) => HttpService = httpService;

    internal BangumiHttpService HttpService { get; }
}
