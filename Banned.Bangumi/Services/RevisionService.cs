using Banned.Bangumi.Exceptions;
using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Revisions;
using Banned.Bangumi.Services.Internal;
using System.Globalization;

namespace Banned.Bangumi.Services;

/// <summary>
/// 提供 Bangumi 修订操作。<br/>
/// Provides operations for Bangumi revisions.
/// </summary>
public sealed class RevisionService
{
    internal RevisionService(BangumiHttpService httpService) => HttpService = httpService;

    internal BangumiHttpService HttpService { get; }

    /// <summary>
    /// 获取指定人物的修订记录。<br/>
    /// Gets revisions for the specified person.
    /// </summary>
    /// <param name="personId">人物 ID。 / Person ID.</param>
    /// <param name="limit">返回数量，允许范围为 1 到 50；为 <see langword="null"/> 时使用服务端默认值。 / Result limit, from 1 through 50, or <see langword="null"/> to use the server default.</param>
    /// <param name="offset">分页偏移量；为 <see langword="null"/> 时使用服务端默认值。 / Pagination offset, or <see langword="null"/> to use the server default.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>分页修订记录。<br/>A page of revisions.</returns>
    /// <exception cref="ArgumentOutOfRangeException">人物 ID 或分页值无效。 / The person ID or a pagination value is invalid.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public Task<PagedResult<Revision>> GetPersons(int               personId, int? limit = null, int? offset = null,
                                                  CancellationToken cancellationToken = default) =>
        GetList("/v0/revisions/persons", "person_id", personId, nameof(personId), "Person", limit, offset,
                cancellationToken);

    /// <summary>
    /// 获取指定人物修订的详情。<br/>
    /// Gets details of the specified person revision.
    /// </summary>
    /// <param name="revisionId">修订 ID。 / Revision ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>人物修订详情。<br/>The person revision details.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="revisionId"/> 小于 1。 / <paramref name="revisionId"/> is less than 1.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public Task<PersonRevision> GetPerson(int revisionId, CancellationToken cancellationToken = default) =>
        GetDetail<PersonRevision>("persons", revisionId, cancellationToken);

    /// <summary>
    /// 获取指定角色的修订记录。<br/>
    /// Gets revisions for the specified character.
    /// </summary>
    /// <param name="characterId">角色 ID。 / Character ID.</param>
    /// <param name="limit">返回数量，允许范围为 1 到 50；为 <see langword="null"/> 时使用服务端默认值。 / Result limit, from 1 through 50, or <see langword="null"/> to use the server default.</param>
    /// <param name="offset">分页偏移量；为 <see langword="null"/> 时使用服务端默认值。 / Pagination offset, or <see langword="null"/> to use the server default.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>分页修订记录。<br/>A page of revisions.</returns>
    /// <exception cref="ArgumentOutOfRangeException">角色 ID 或分页值无效。 / The character ID or a pagination value is invalid.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public Task<PagedResult<Revision>> GetCharacters(int characterId, int? limit = null, int? offset = null,
                                                     CancellationToken cancellationToken = default) =>
        GetList("/v0/revisions/characters", "character_id", characterId, nameof(characterId), "Character", limit,
                offset, cancellationToken);

    /// <summary>
    /// 获取指定角色修订的详情。<br/>
    /// Gets details of the specified character revision.
    /// </summary>
    /// <param name="revisionId">修订 ID。 / Revision ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>角色修订详情。<br/>The character revision details.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="revisionId"/> 小于 1。 / <paramref name="revisionId"/> is less than 1.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public Task<CharacterRevision> GetCharacter(int revisionId, CancellationToken cancellationToken = default) =>
        GetDetail<CharacterRevision>("characters", revisionId, cancellationToken);

    /// <summary>
    /// 获取指定条目的修订记录。<br/>
    /// Gets revisions for the specified subject.
    /// </summary>
    /// <param name="subjectId">条目 ID。 / Subject ID.</param>
    /// <param name="limit">返回数量，允许范围为 1 到 50；为 <see langword="null"/> 时使用服务端默认值。 / Result limit, from 1 through 50, or <see langword="null"/> to use the server default.</param>
    /// <param name="offset">分页偏移量；为 <see langword="null"/> 时使用服务端默认值。 / Pagination offset, or <see langword="null"/> to use the server default.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>分页修订记录。<br/>A page of revisions.</returns>
    /// <exception cref="ArgumentOutOfRangeException">条目 ID 或分页值无效。 / The subject ID or a pagination value is invalid.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public Task<PagedResult<Revision>> GetSubjects(int               subjectId, int? limit = null, int? offset = null,
                                                   CancellationToken cancellationToken = default) =>
        GetList("/v0/revisions/subjects", "subject_id", subjectId, nameof(subjectId), "Subject", limit, offset,
                cancellationToken);

    /// <summary>
    /// 获取指定条目修订的详情。<br/>
    /// Gets details of the specified subject revision.
    /// </summary>
    /// <param name="revisionId">修订 ID。 / Revision ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>条目修订详情。<br/>The subject revision details.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="revisionId"/> 小于 1。 / <paramref name="revisionId"/> is less than 1.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public Task<SubjectRevision> GetSubject(int revisionId, CancellationToken cancellationToken = default) =>
        GetDetail<SubjectRevision>("subjects", revisionId, cancellationToken);

    /// <summary>
    /// 获取指定章节的修订记录。<br/>
    /// Gets revisions for the specified episode.
    /// </summary>
    /// <param name="episodeId">章节 ID。 / Episode ID.</param>
    /// <param name="limit">返回数量，允许范围为 1 到 50；为 <see langword="null"/> 时使用服务端默认值。 / Result limit, from 1 through 50, or <see langword="null"/> to use the server default.</param>
    /// <param name="offset">分页偏移量；为 <see langword="null"/> 时使用服务端默认值。 / Pagination offset, or <see langword="null"/> to use the server default.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>分页修订记录。<br/>A page of revisions.</returns>
    /// <exception cref="ArgumentOutOfRangeException">章节 ID 或分页值无效。 / The episode ID or a pagination value is invalid.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public Task<PagedResult<Revision>> GetEpisodes(int               episodeId, int? limit = null, int? offset = null,
                                                   CancellationToken cancellationToken = default) =>
        GetList("/v0/revisions/episodes", "episode_id", episodeId, nameof(episodeId), "Episode", limit, offset,
                cancellationToken);

    /// <summary>
    /// 获取指定章节修订的详情。<br/>
    /// Gets details of the specified episode revision.
    /// </summary>
    /// <param name="revisionId">修订 ID。 / Revision ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>章节修订详情。<br/>The episode revision details.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="revisionId"/> 小于 1。 / <paramref name="revisionId"/> is less than 1.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public Task<EpisodeRevision> GetEpisode(int revisionId, CancellationToken cancellationToken = default) =>
        GetDetail<EpisodeRevision>("episodes", revisionId, cancellationToken);

    private async Task<PagedResult<Revision>> GetList(string path, string resourceIdName, int resourceId,
                                                      string parameterName, string displayName, int? limit, int? offset,
                                                      CancellationToken cancellationToken)
    {
        ValidateId(resourceId, parameterName, displayName);
        ValidatePagination(limit, offset);

        path = BangumiHttpService.AddQueryString(path, new Dictionary<string, string?>
        {
            [resourceIdName] = resourceId.ToString(CultureInfo.InvariantCulture),
            ["limit"]        = FormatInteger(limit),
            ["offset"]       = FormatInteger(offset)
        });

        return await HttpService.Get<PagedResult<Revision>>(path, AuthenticationMode.None, cancellationToken)
                                .ConfigureAwait(false);
    }

    private async Task<TRevision> GetDetail<TRevision>(string            resourcePath, int revisionId,
                                                       CancellationToken cancellationToken)
    {
        ValidateId(revisionId, nameof(revisionId), "Revision");
        return await HttpService
                    .Get<TRevision>($"/v0/revisions/{resourcePath}/{revisionId}", AuthenticationMode.None,
                                    cancellationToken).ConfigureAwait(false);
    }

    private static string? FormatInteger(int? value) => value?.ToString(CultureInfo.InvariantCulture);

    private static void ValidatePagination(int? limit, int? offset)
    {
        if (limit is < 1 or > 50)
        {
            throw new ArgumentOutOfRangeException(nameof(limit), limit, "Limit must be from 1 through 50.");
        }

        if (offset is < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), offset, "Offset cannot be negative.");
        }
    }

    private static void ValidateId(int value, string parameterName, string displayName)
    {
        if (value < 1)
        {
            throw new ArgumentOutOfRangeException(parameterName, value, $"{displayName} ID must be positive.");
        }
    }
}
