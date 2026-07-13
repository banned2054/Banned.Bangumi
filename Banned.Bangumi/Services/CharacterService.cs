using Banned.Bangumi.Services.Internal;

namespace Banned.Bangumi.Services;

/// <summary>
/// 提供 Bangumi 角色操作。<br/>
/// Provides operations for Bangumi characters.
/// </summary>
public sealed class CharacterService
{
    internal CharacterService(BangumiHttpService httpService) => HttpService = httpService;

    internal BangumiHttpService HttpService { get; }
}
