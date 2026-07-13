using Banned.Bangumi.Services.Internal;

namespace Banned.Bangumi.Services;

/// <summary>
/// 提供 Bangumi 人物操作。<br/>
/// Provides operations for Bangumi persons.
/// </summary>
public sealed class PersonService
{
    internal PersonService(BangumiHttpService httpService) => HttpService = httpService;

    internal BangumiHttpService HttpService { get; }
}
