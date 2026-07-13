using Banned.Bangumi.Exceptions;
using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Episodes;
using Banned.Bangumi.Services.Internal;
using System.Globalization;

namespace Banned.Bangumi.Services;

/// <summary>
/// 提供 Bangumi 章节操作。<br/>
/// Provides operations for Bangumi episodes.
/// </summary>
public sealed class EpisodeService
{
    internal EpisodeService(BangumiHttpService httpService) => HttpService = httpService;

    internal BangumiHttpService HttpService { get; }

    /// <summary>
    /// 按条目和可选类型浏览章节。<br/>
    /// Browses episodes by subject and optional type.
    /// </summary>
    /// <param name="request">浏览条件。 / Browse criteria.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>分页章节结果。<br/>A page of episodes.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="request"/> 为 <see langword="null"/>。 / <paramref name="request"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">请求包含无效的条目 ID、章节类型或分页值。 / The request contains an invalid subject ID, episode type, or pagination value.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<PagedResult<Episode>> Browse(EpisodeBrowseRequest request,
                                                   CancellationToken    cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ValidateBrowseRequest(request);

        var path = BangumiHttpService.AddQueryString("/v0/episodes", new Dictionary<string, string?>
        {
            ["subject_id"] = request.SubjectId.ToString(CultureInfo.InvariantCulture),
            ["type"] = request.Type is { } type
                ? ((int)type).ToString(CultureInfo.InvariantCulture)
                : null,
            ["limit"]  = FormatInteger(request.Limit),
            ["offset"] = FormatInteger(request.Offset)
        });

        return await HttpService.Get<PagedResult<Episode>>(path, AuthenticationMode.Optional, cancellationToken)
                                .ConfigureAwait(false);
    }

    /// <summary>
    /// 获取指定章节。<br/>
    /// Gets the specified episode.
    /// </summary>
    /// <param name="episodeId">章节 ID。 / Episode ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>章节详情。<br/>The episode details.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="episodeId"/> 小于 1。 / <paramref name="episodeId"/> is less than 1.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<EpisodeDetail> Get(int episodeId, CancellationToken cancellationToken = default)
    {
        if (episodeId < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(episodeId), episodeId, "Episode ID must be positive.");
        }

        return await HttpService
                    .Get<EpisodeDetail>($"/v0/episodes/{episodeId}", AuthenticationMode.Optional, cancellationToken)
                    .ConfigureAwait(false);
    }

    private static string? FormatInteger(int? value) => value?.ToString(CultureInfo.InvariantCulture);

    private static void ValidateBrowseRequest(EpisodeBrowseRequest request)
    {
        if (request.SubjectId < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(request), request.SubjectId,
                                                  "Subject ID must be positive.");
        }

        if (request.Type is { } type && !Enum.IsDefined(type))
        {
            throw new ArgumentOutOfRangeException(nameof(request), type, "The episode type is invalid.");
        }

        if (request.Limit is < 1 or > 200)
        {
            throw new ArgumentOutOfRangeException(nameof(request), request.Limit,
                                                  "Limit must be from 1 through 200.");
        }

        if (request.Offset is < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request), request.Offset, "Offset cannot be negative.");
        }
    }
}
