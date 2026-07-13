using Banned.Bangumi.Services.Internal;

namespace Banned.Bangumi.Services;

/// <summary>
/// 提供 Bangumi 收藏操作。<br/>
/// Provides operations for Bangumi collections.
/// </summary>
public sealed class CollectionService
{
    internal CollectionService(BangumiHttpService httpService) => HttpService = httpService;

    internal BangumiHttpService HttpService { get; }
}
