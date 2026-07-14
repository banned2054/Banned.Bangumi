using Banned.Bangumi.Exceptions;
using Banned.Bangumi.Models.Collections;
using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Services.Internal;
using System.Globalization;

namespace Banned.Bangumi.Services;

/// <summary>
/// 提供 Bangumi 收藏操作。<br/>
/// Provides operations for Bangumi collections.
/// </summary>
public sealed class CollectionService
{
    internal CollectionService(BangumiHttpService httpService) => HttpService = httpService;

    internal BangumiHttpService HttpService { get; }

    /// <summary>
    /// 按条件浏览指定用户的条目收藏。<br/>
    /// Browses the specified user's subject collections using the specified criteria.
    /// </summary>
    /// <param name="username">用户名。 / Username.</param>
    /// <param name="request">浏览条件；为 <see langword="null"/> 时不添加筛选条件。 / Browse criteria, or <see langword="null"/> to add no filters.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>分页条目收藏结果。<br/>A page of subject collections.</returns>
    /// <exception cref="ArgumentException">用户名为空，或请求包含无效值。 / The username is empty or the request contains an invalid value.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="username"/> 为 <see langword="null"/>。 / <paramref name="username"/> is <see langword="null"/>.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<PagedResult<UserSubjectCollection>> BrowseSubjects(
        string username, SubjectCollectionBrowseRequest? request = null, CancellationToken cancellationToken = default)
    {
        ValidateUsername(username);
        request ??= new SubjectCollectionBrowseRequest();
        ValidateSubjectBrowseRequest(request);

        var path = BangumiHttpService.AddQueryString($"/v0/users/{Uri.EscapeDataString(username)}/collections",
                                                     new Dictionary<string, string?>
                                                     {
                                                         ["subject_type"] = FormatEnum(request.SubjectType),
                                                         ["type"]         = FormatEnum(request.Type),
                                                         ["limit"]        = FormatInteger(request.Limit),
                                                         ["offset"]       = FormatInteger(request.Offset)
                                                     });

        return await HttpService
                    .Get<PagedResult<UserSubjectCollection>>(path, AuthenticationMode.Optional, cancellationToken)
                    .ConfigureAwait(false);
    }

    /// <summary>
    /// 获取指定用户的单个条目收藏。<br/>
    /// Gets a single subject collection for the specified user.
    /// </summary>
    /// <param name="username">用户名。 / Username.</param>
    /// <param name="subjectId">条目 ID。 / Subject ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>条目收藏信息。<br/>The subject collection information.</returns>
    /// <exception cref="ArgumentException"><paramref name="username"/> 为空。 / <paramref name="username"/> is empty.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="username"/> 为 <see langword="null"/>。 / <paramref name="username"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="subjectId"/> 小于 1。 / <paramref name="subjectId"/> is less than 1.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<UserSubjectCollection> GetSubject(string            username,
                                                        int               subjectId,
                                                        CancellationToken cancellationToken = default)
    {
        ValidateUsername(username);
        ValidateId(subjectId, nameof(subjectId), "Subject");

        return await HttpService
                    .Get<UserSubjectCollection>($"/v0/users/{Uri.EscapeDataString(username)}/collections/{subjectId}",
                                                AuthenticationMode.Optional, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 新增或修改当前用户的条目收藏。<br/>
    /// Creates or updates a subject collection for the current user.
    /// </summary>
    /// <param name="subjectId">条目 ID。 / Subject ID.</param>
    /// <param name="request">收藏信息。 / Collection information.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>表示异步操作的任务。<br/>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="request"/> 为 <see langword="null"/>。 / <paramref name="request"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">条目 ID、收藏状态、评分或进度无效。 / The subject ID, collection status, rating, or progress is invalid.</exception>
    /// <exception cref="ArgumentException">标签为空或包含空白字符。 / A tag is empty or contains whitespace.</exception>
    /// <exception cref="BangumiAuthenticationException">未配置访问令牌或 API 拒绝鉴权。 / No access token is configured or the API rejects authentication.</exception>
    /// <exception cref="BangumiApiException">API 返回错误。 / The API returns an error.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task CreateOrUpdateSubject(int                            subjectId,
                                            SubjectCollectionUpdateRequest request,
                                            CancellationToken              cancellationToken = default)
    {
        ValidateId(subjectId, nameof(subjectId), "Subject");
        ValidateSubjectUpdateRequest(request);

        await HttpService
             .Send(HttpMethod.Post, $"/v0/users/-/collections/{subjectId}", AuthenticationMode.Required, request,
                   cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 修改当前用户已有的条目收藏。<br/>
    /// Updates an existing subject collection for the current user.
    /// </summary>
    /// <param name="subjectId">条目 ID。 / Subject ID.</param>
    /// <param name="request">要修改的收藏信息。 / Collection information to update.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>表示异步操作的任务。<br/>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="request"/> 为 <see langword="null"/>。 / <paramref name="request"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">条目 ID、收藏状态、评分或进度无效。 / The subject ID, collection status, rating, or progress is invalid.</exception>
    /// <exception cref="ArgumentException">标签为空或包含空白字符。 / A tag is empty or contains whitespace.</exception>
    /// <exception cref="BangumiAuthenticationException">未配置访问令牌或 API 拒绝鉴权。 / No access token is configured or the API rejects authentication.</exception>
    /// <exception cref="BangumiApiException">API 返回错误。 / The API returns an error.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task UpdateSubject(int                            subjectId,
                                    SubjectCollectionUpdateRequest request,
                                    CancellationToken              cancellationToken = default)
    {
        ValidateId(subjectId, nameof(subjectId), "Subject");
        ValidateSubjectUpdateRequest(request);

        await HttpService
             .Send(HttpMethod.Patch, $"/v0/users/-/collections/{subjectId}", AuthenticationMode.Required, request,
                   cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 浏览当前用户在指定条目下的章节收藏。<br/>
    /// Browses the current user's episode collections for the specified subject.
    /// </summary>
    /// <param name="subjectId">条目 ID。 / Subject ID.</param>
    /// <param name="request">浏览条件；为 <see langword="null"/> 时不添加筛选条件。 / Browse criteria, or <see langword="null"/> to add no filters.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>分页章节收藏结果。<br/>A page of episode collections.</returns>
    /// <exception cref="ArgumentOutOfRangeException">条目 ID、章节类型或分页值无效。 / The subject ID, episode type, or pagination value is invalid.</exception>
    /// <exception cref="BangumiAuthenticationException">未配置访问令牌或 API 拒绝鉴权。 / No access token is configured or the API rejects authentication.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<PagedResult<UserEpisodeCollection>> BrowseEpisodes(
        int subjectId, EpisodeCollectionBrowseRequest? request = null, CancellationToken cancellationToken = default)
    {
        ValidateId(subjectId, nameof(subjectId), "Subject");
        request ??= new EpisodeCollectionBrowseRequest();
        ValidateEpisodeBrowseRequest(request);

        var path = BangumiHttpService.AddQueryString($"/v0/users/-/collections/{subjectId}/episodes",
                                                     new Dictionary<string, string?>
                                                     {
                                                         ["offset"]       = FormatInteger(request.Offset),
                                                         ["limit"]        = FormatInteger(request.Limit),
                                                         ["episode_type"] = FormatEnum(request.EpisodeType)
                                                     });

        return await HttpService
                    .Get<PagedResult<UserEpisodeCollection>>(path, AuthenticationMode.Required, cancellationToken)
                    .ConfigureAwait(false);
    }

    /// <summary>
    /// 批量修改当前用户在指定条目下的章节收藏状态。<br/>
    /// Updates the current user's episode collection statuses in a batch for the specified subject.
    /// </summary>
    /// <param name="subjectId">条目 ID。 / Subject ID.</param>
    /// <param name="request">批量修改请求。 / Batch update request.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>表示异步操作的任务。<br/>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="request"/> 为 <see langword="null"/>。 / <paramref name="request"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">条目 ID、章节 ID 或收藏状态无效。 / The subject ID, an episode ID, or the collection status is invalid.</exception>
    /// <exception cref="BangumiAuthenticationException">未配置访问令牌或 API 拒绝鉴权。 / No access token is configured or the API rejects authentication.</exception>
    /// <exception cref="BangumiApiException">API 返回错误。 / The API returns an error.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task UpdateEpisodes(int                                 subjectId,
                                     EpisodeCollectionBatchUpdateRequest request,
                                     CancellationToken                   cancellationToken = default)
    {
        ValidateId(subjectId, nameof(subjectId), "Subject");
        ValidateEpisodeBatchUpdateRequest(request);

        await HttpService
             .Send(HttpMethod.Patch, $"/v0/users/-/collections/{subjectId}/episodes", AuthenticationMode.Required,
                   request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 获取当前用户的单个章节收藏。<br/>
    /// Gets a single episode collection for the current user.
    /// </summary>
    /// <param name="episodeId">章节 ID。 / Episode ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>章节收藏信息。<br/>The episode collection information.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="episodeId"/> 小于 1。 / <paramref name="episodeId"/> is less than 1.</exception>
    /// <exception cref="BangumiAuthenticationException">未配置访问令牌或 API 拒绝鉴权。 / No access token is configured or the API rejects authentication.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<UserEpisodeCollection> GetEpisode(int episodeId, CancellationToken cancellationToken = default)
    {
        ValidateId(episodeId, nameof(episodeId), "Episode");
        return await HttpService
                    .Get<UserEpisodeCollection>($"/v0/users/-/collections/-/episodes/{episodeId}",
                                                AuthenticationMode.Required, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 修改当前用户的单个章节收藏状态。<br/>
    /// Updates a single episode collection status for the current user.
    /// </summary>
    /// <param name="episodeId">章节 ID。 / Episode ID.</param>
    /// <param name="request">修改请求。 / Update request.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>表示异步操作的任务。<br/>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="request"/> 为 <see langword="null"/>。 / <paramref name="request"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">章节 ID 或收藏状态无效。 / The episode ID or collection status is invalid.</exception>
    /// <exception cref="BangumiAuthenticationException">未配置访问令牌或 API 拒绝鉴权。 / No access token is configured or the API rejects authentication.</exception>
    /// <exception cref="BangumiApiException">API 返回错误。 / The API returns an error.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task UpdateEpisode(int                            episodeId,
                                    EpisodeCollectionUpdateRequest request,
                                    CancellationToken              cancellationToken = default)
    {
        ValidateId(episodeId, nameof(episodeId), "Episode");
        ArgumentNullException.ThrowIfNull(request);
        ValidateEnum(request.Type, nameof(request), "episode collection status");

        await HttpService
             .Send(HttpMethod.Put, $"/v0/users/-/collections/-/episodes/{episodeId}", AuthenticationMode.Required,
                   request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 获取指定用户收藏的角色。<br/>
    /// Gets characters collected by the specified user.
    /// </summary>
    /// <param name="username">用户名。 / Username.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>分页角色收藏结果。<br/>A page of character collections.</returns>
    /// <exception cref="ArgumentException"><paramref name="username"/> 为空。 / <paramref name="username"/> is empty.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="username"/> 为 <see langword="null"/>。 / <paramref name="username"/> is <see langword="null"/>.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<PagedResult<UserCharacterCollection>> GetCharacters(
        string            username,
        CancellationToken cancellationToken = default)
    {
        ValidateUsername(username);
        return await HttpService
                    .Get<PagedResult<UserCharacterCollection>>(
                                                               $"/v0/users/{Uri.EscapeDataString(username)}/collections/-/characters",
                                                               AuthenticationMode.None, cancellationToken)
                    .ConfigureAwait(false);
    }

    /// <summary>
    /// 获取指定用户的单个角色收藏。<br/>
    /// Gets a single character collection for the specified user.
    /// </summary>
    /// <param name="username">用户名。 / Username.</param>
    /// <param name="characterId">角色 ID。 / Character ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>角色收藏信息。<br/>The character collection information.</returns>
    /// <exception cref="ArgumentException"><paramref name="username"/> 为空。 / <paramref name="username"/> is empty.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="username"/> 为 <see langword="null"/>。 / <paramref name="username"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="characterId"/> 小于 1。 / <paramref name="characterId"/> is less than 1.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<UserCharacterCollection> GetCharacter(string            username,
                                                            int               characterId,
                                                            CancellationToken cancellationToken = default)
    {
        ValidateUsername(username);
        ValidateId(characterId, nameof(characterId), "Character");
        return await HttpService
                    .Get<UserCharacterCollection>(
                                                  $"/v0/users/{Uri.EscapeDataString(username)}/collections/-/characters/{characterId}",
                                                  AuthenticationMode.None, cancellationToken)
                    .ConfigureAwait(false);
    }

    /// <summary>
    /// 获取指定用户收藏的人物。<br/>
    /// Gets persons collected by the specified user.
    /// </summary>
    /// <param name="username">用户名。 / Username.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>分页人物收藏结果。<br/>A page of person collections.</returns>
    /// <exception cref="ArgumentException"><paramref name="username"/> 为空。 / <paramref name="username"/> is empty.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="username"/> 为 <see langword="null"/>。 / <paramref name="username"/> is <see langword="null"/>.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<PagedResult<UserPersonCollection>> GetPersons(string            username,
                                                                    CancellationToken cancellationToken = default)
    {
        ValidateUsername(username);
        return await HttpService
                    .Get<PagedResult<UserPersonCollection>>(
                                                            $"/v0/users/{Uri.EscapeDataString(username)}/collections/-/persons",
                                                            AuthenticationMode.None, cancellationToken)
                    .ConfigureAwait(false);
    }

    /// <summary>
    /// 获取指定用户的单个人物收藏。<br/>
    /// Gets a single person collection for the specified user.
    /// </summary>
    /// <param name="username">用户名。 / Username.</param>
    /// <param name="personId">人物 ID。 / Person ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>人物收藏信息。<br/>The person collection information.</returns>
    /// <exception cref="ArgumentException"><paramref name="username"/> 为空。 / <paramref name="username"/> is empty.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="username"/> 为 <see langword="null"/>。 / <paramref name="username"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="personId"/> 小于 1。 / <paramref name="personId"/> is less than 1.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<UserPersonCollection> GetPerson(string            username,
                                                      int               personId,
                                                      CancellationToken cancellationToken = default)
    {
        ValidateUsername(username);
        ValidateId(personId, nameof(personId), "Person");
        return await HttpService
                    .Get<UserPersonCollection>(
                                               $"/v0/users/{Uri.EscapeDataString(username)}/collections/-/persons/{personId}",
                                               AuthenticationMode.None, cancellationToken).ConfigureAwait(false);
    }

    private static string? FormatEnum<TEnum>(TEnum? value) where TEnum : struct, Enum =>
        value is { } enumValue
            ? Convert.ToInt32(enumValue, CultureInfo.InvariantCulture)
                     .ToString(CultureInfo.InvariantCulture)
            : null;

    private static string? FormatInteger(int? value) => value?.ToString(CultureInfo.InvariantCulture);

    private static void ValidateEpisodeBatchUpdateRequest(EpisodeCollectionBatchUpdateRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.EpisodeIds);
        ValidateEnum(request.Type, nameof(request), "episode collection status");

        foreach (var episodeId in request.EpisodeIds)
        {
            ValidateId(episodeId, nameof(request), "Episode");
        }
    }

    private static void ValidateEpisodeBrowseRequest(EpisodeCollectionBrowseRequest request)
    {
        if (request.EpisodeType is { } episodeType && !Enum.IsDefined(episodeType))
        {
            throw new ArgumentOutOfRangeException(nameof(request), episodeType, "The episode type is invalid.");
        }

        if (request.Limit is < 1 or > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(request), request.Limit, "Limit must be from 1 through 1000.");
        }

        if (request.Offset is < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request), request.Offset, "Offset cannot be negative.");
        }
    }

    private static void ValidateSubjectBrowseRequest(SubjectCollectionBrowseRequest request)
    {
        if (request.SubjectType is { } subjectType && !Enum.IsDefined(subjectType))
        {
            throw new ArgumentOutOfRangeException(nameof(request), subjectType, "The subject type is invalid.");
        }

        if (request.Type is { } collectionType && !Enum.IsDefined(collectionType))
        {
            throw new ArgumentOutOfRangeException(nameof(request), collectionType,
                                                  "The subject collection status is invalid.");
        }

        if (request.Limit is < 1 or > 50)
        {
            throw new ArgumentOutOfRangeException(nameof(request), request.Limit, "Limit must be from 1 through 50.");
        }

        if (request.Offset is < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request), request.Offset, "Offset cannot be negative.");
        }
    }

    private static void ValidateSubjectUpdateRequest(SubjectCollectionUpdateRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Type is { } type)
        {
            ValidateEnum(type, nameof(request), "subject collection status");
        }

        if (request.Rate is < 0 or > 10)
        {
            throw new ArgumentOutOfRangeException(nameof(request), request.Rate, "Rate must be from 0 through 10.");
        }

        if (request.EpisodeStatus is < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request), request.EpisodeStatus,
                                                  "Episode status cannot be negative.");
        }

        if (request.VolumeStatus is < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request), request.VolumeStatus,
                                                  "Volume status cannot be negative.");
        }

        if (request.Tags is null)
        {
            return;
        }

        foreach (var tag in request.Tags)
        {
            if (string.IsNullOrWhiteSpace(tag) || tag.Any(char.IsWhiteSpace))
            {
                throw new ArgumentException("Tags must be non-empty and cannot contain whitespace.", nameof(request));
            }
        }
    }

    private static void ValidateEnum<TEnum>(TEnum value, string parameterName, string displayName)
        where TEnum : struct, Enum
    {
        if (!Enum.IsDefined(value))
        {
            throw new ArgumentOutOfRangeException(parameterName, value, $"The {displayName} is invalid.");
        }
    }

    private static void ValidateId(int value, string parameterName, string displayName)
    {
        if (value < 1)
        {
            throw new ArgumentOutOfRangeException(parameterName, value, $"{displayName} ID must be positive.");
        }
    }

    private static void ValidateUsername(string username) => ArgumentException.ThrowIfNullOrWhiteSpace(username);
}
