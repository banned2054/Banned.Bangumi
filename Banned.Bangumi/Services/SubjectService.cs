using Banned.Bangumi.Services.Internal;

namespace Banned.Bangumi.Services;

/// <summary>
/// 提供 Bangumi 条目操作。<br/>
/// Provides operations for Bangumi subjects.
/// </summary>
public sealed class SubjectService
{
    internal SubjectService(BangumiHttpService httpService) => HttpService = httpService;

    internal BangumiHttpService HttpService { get; }
}
