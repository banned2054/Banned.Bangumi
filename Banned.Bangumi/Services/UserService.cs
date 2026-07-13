using Banned.Bangumi.Services.Internal;

namespace Banned.Bangumi.Services;

/// <summary>
/// 提供 Bangumi 用户操作。<br/>
/// Provides operations for Bangumi users.
/// </summary>
public sealed class UserService
{
    internal UserService(BangumiHttpService httpService) => HttpService = httpService;

    internal BangumiHttpService HttpService { get; }
}
