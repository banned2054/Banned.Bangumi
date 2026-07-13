using Banned.Bangumi.Services.Internal;

namespace Banned.Bangumi.Services;

/// <summary>
/// 提供 Bangumi 修订操作。<br/>
/// Provides operations for Bangumi revisions.
/// </summary>
public sealed class RevisionService
{
    internal RevisionService(BangumiHttpService httpService) => HttpService = httpService;

    internal BangumiHttpService HttpService { get; }
}
