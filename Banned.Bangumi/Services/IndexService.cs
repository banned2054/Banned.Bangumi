using Banned.Bangumi.Services.Internal;

namespace Banned.Bangumi.Services;

/// <summary>
/// 提供 Bangumi 目录操作。<br/>
/// Provides operations for Bangumi indices.
/// </summary>
public sealed class IndexService
{
    internal IndexService(BangumiHttpService httpService) => HttpService = httpService;

    internal BangumiHttpService HttpService { get; }
}
