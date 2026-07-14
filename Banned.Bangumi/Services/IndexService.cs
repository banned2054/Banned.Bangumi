using Banned.Bangumi.Exceptions;
using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Indices;
using Banned.Bangumi.Models.Subjects;
using Banned.Bangumi.Services.Internal;
using System.Globalization;

namespace Banned.Bangumi.Services;

/// <summary>
/// 提供 Bangumi 目录操作。<br/>
/// Provides operations for Bangumi indices.
/// </summary>
public sealed class IndexService
{
    internal IndexService(BangumiHttpService httpService) => HttpService = httpService;

    internal BangumiHttpService HttpService { get; }

    /// <summary>
    /// 为当前用户新建目录。<br/>
    /// Creates a new index for the current user.
    /// </summary>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>新建的目录。<br/>The newly created index.</returns>
    /// <exception cref="BangumiAuthenticationException">未配置访问令牌或 API 拒绝鉴权。 / No access token is configured or the API rejects authentication.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<BangumiIndex> Create(CancellationToken cancellationToken = default) => await HttpService
       .Send<BangumiIndex>(HttpMethod.Post, "/v0/indices", AuthenticationMode.Required,
                           cancellationToken : cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// 获取指定目录。<br/>
    /// Gets the specified index.
    /// </summary>
    /// <param name="indexId">目录 ID。 / Index ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>目录详情。<br/>The index details.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexId"/> 小于 1。 / <paramref name="indexId"/> is less than 1.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<BangumiIndex> Get(int indexId, CancellationToken cancellationToken = default)
    {
        ValidateId(indexId, nameof(indexId), "Index");
        return await HttpService
                    .Get<BangumiIndex>($"/v0/indices/{indexId}", AuthenticationMode.Optional, cancellationToken)
                    .ConfigureAwait(false);
    }

    /// <summary>
    /// 修改指定目录的信息。<br/>
    /// Updates information for the specified index.
    /// </summary>
    /// <param name="indexId">目录 ID。 / Index ID.</param>
    /// <param name="request">要修改的目录信息。 / Index information to update.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>修改后的目录。<br/>The updated index.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="request"/> 为 <see langword="null"/>。 / <paramref name="request"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexId"/> 小于 1。 / <paramref name="indexId"/> is less than 1.</exception>
    /// <exception cref="BangumiAuthenticationException">未配置访问令牌或 API 拒绝鉴权。 / No access token is configured or the API rejects authentication.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<BangumiIndex> Update(int               indexId, IndexUpdateRequest request,
                                           CancellationToken cancellationToken = default)
    {
        ValidateId(indexId, nameof(indexId), "Index");
        ArgumentNullException.ThrowIfNull(request);

        return await HttpService
                    .Send<BangumiIndex>(HttpMethod.Put, $"/v0/indices/{indexId}", AuthenticationMode.Required, request,
                                        cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 获取指定目录中的条目。<br/>
    /// Gets subjects in the specified index.
    /// </summary>
    /// <param name="indexId">目录 ID。 / Index ID.</param>
    /// <param name="type">条目类型；为 <see langword="null"/> 时不按类型筛选。 / Subject type, or <see langword="null"/> to include all types.</param>
    /// <param name="limit">返回数量，允许范围为 1 到 50；为 <see langword="null"/> 时使用服务端默认值。 / Result limit, from 1 through 50, or <see langword="null"/> to use the server default.</param>
    /// <param name="offset">分页偏移量；为 <see langword="null"/> 时使用服务端默认值。 / Pagination offset, or <see langword="null"/> to use the server default.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>分页目录条目结果。<br/>A page of index subjects.</returns>
    /// <exception cref="ArgumentOutOfRangeException">目录 ID、条目类型或分页值无效。 / The index ID, subject type, or a pagination value is invalid.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<PagedResult<IndexSubject>> GetSubjects(int               indexId,
                                                             SubjectType?      type              = null,
                                                             int?              limit             = null,
                                                             int?              offset            = null,
                                                             CancellationToken cancellationToken = default)
    {
        ValidateId(indexId, nameof(indexId), "Index");
        ValidateSubjectType(type);
        ValidatePagination(limit, offset);

        var path = BangumiHttpService.AddQueryString($"/v0/indices/{indexId}/subjects",
                                                     new Dictionary<string, string?>
                                                     {
                                                         ["type"]   = FormatEnum(type),
                                                         ["limit"]  = FormatInteger(limit),
                                                         ["offset"] = FormatInteger(offset)
                                                     });

        return await HttpService
                    .Get<PagedResult<IndexSubject>>(path, AuthenticationMode.Optional, cancellationToken)
                    .ConfigureAwait(false);
    }

    /// <summary>
    /// 向指定目录添加条目。<br/>
    /// Adds a subject to the specified index.
    /// </summary>
    /// <param name="indexId">目录 ID。 / Index ID.</param>
    /// <param name="request">要添加的条目信息。 / Subject information to add.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>表示异步操作的任务。<br/>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="request"/> 为 <see langword="null"/>。 / <paramref name="request"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">目录 ID 或条目 ID 小于 1。 / The index ID or subject ID is less than 1.</exception>
    /// <exception cref="BangumiAuthenticationException">未配置访问令牌或 API 拒绝鉴权。 / No access token is configured or the API rejects authentication.</exception>
    /// <exception cref="BangumiApiException">API 返回错误。 / The API returns an error.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task AddSubject(int                    indexId,
                                 IndexSubjectAddRequest request,
                                 CancellationToken      cancellationToken = default)
    {
        ValidateId(indexId, nameof(indexId), "Index");
        ArgumentNullException.ThrowIfNull(request);
        ValidateId(request.SubjectId, nameof(request), "Subject");

        await HttpService
             .Send(HttpMethod.Post, $"/v0/indices/{indexId}/subjects", AuthenticationMode.Required, request,
                   cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 新增或修改指定目录中的条目信息。<br/>
    /// Creates or updates subject information in the specified index.
    /// </summary>
    /// <param name="indexId">目录 ID。 / Index ID.</param>
    /// <param name="subjectId">条目 ID。 / Subject ID.</param>
    /// <param name="request">要新增或修改的条目信息。 / Subject information to create or update.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>表示异步操作的任务。<br/>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="request"/> 为 <see langword="null"/>。 / <paramref name="request"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">目录 ID 或条目 ID 小于 1。 / The index ID or subject ID is less than 1.</exception>
    /// <exception cref="BangumiAuthenticationException">未配置访问令牌或 API 拒绝鉴权。 / No access token is configured or the API rejects authentication.</exception>
    /// <exception cref="BangumiApiException">API 返回错误。 / The API returns an error.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task CreateOrUpdateSubject(int                       indexId,
                                            int                       subjectId,
                                            IndexSubjectUpdateRequest request,
                                            CancellationToken         cancellationToken = default)
    {
        ValidateId(indexId, nameof(indexId), "Index");
        ValidateId(subjectId, nameof(subjectId), "Subject");
        ArgumentNullException.ThrowIfNull(request);

        await HttpService
             .Send(HttpMethod.Put, $"/v0/indices/{indexId}/subjects/{subjectId}", AuthenticationMode.Required,
                   request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 从指定目录移除条目。<br/>
    /// Removes a subject from the specified index.
    /// </summary>
    /// <param name="indexId">目录 ID。 / Index ID.</param>
    /// <param name="subjectId">条目 ID。 / Subject ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>表示异步操作的任务。<br/>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentOutOfRangeException">目录 ID 或条目 ID 小于 1。 / The index ID or subject ID is less than 1.</exception>
    /// <exception cref="BangumiAuthenticationException">未配置访问令牌或 API 拒绝鉴权。 / No access token is configured or the API rejects authentication.</exception>
    /// <exception cref="BangumiApiException">API 返回错误。 / The API returns an error.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task RemoveSubject(int indexId, int subjectId, CancellationToken cancellationToken = default)
    {
        ValidateId(indexId, nameof(indexId), "Index");
        ValidateId(subjectId, nameof(subjectId), "Subject");

        await HttpService
             .Send(HttpMethod.Delete, $"/v0/indices/{indexId}/subjects/{subjectId}", AuthenticationMode.Required,
                   cancellationToken : cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 为当前用户收藏指定目录。<br/>
    /// Collects the specified index for the current user.
    /// </summary>
    /// <param name="indexId">目录 ID。 / Index ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>表示异步操作的任务。<br/>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexId"/> 小于 1。 / <paramref name="indexId"/> is less than 1.</exception>
    /// <exception cref="BangumiAuthenticationException">未配置访问令牌或 API 拒绝鉴权。 / No access token is configured or the API rejects authentication.</exception>
    /// <exception cref="BangumiApiException">API 返回错误。 / The API returns an error.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task AddToCollection(int indexId, CancellationToken cancellationToken = default)
    {
        ValidateId(indexId, nameof(indexId), "Index");
        await HttpService
             .Send(HttpMethod.Post, $"/v0/indices/{indexId}/collect", AuthenticationMode.Required,
                   cancellationToken : cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 为当前用户取消收藏指定目录。<br/>
    /// Removes the specified index from the current user's collection.
    /// </summary>
    /// <param name="indexId">目录 ID。 / Index ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>表示异步操作的任务。<br/>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexId"/> 小于 1。 / <paramref name="indexId"/> is less than 1.</exception>
    /// <exception cref="BangumiAuthenticationException">未配置访问令牌或 API 拒绝鉴权。 / No access token is configured or the API rejects authentication.</exception>
    /// <exception cref="BangumiApiException">API 返回错误。 / The API returns an error.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task RemoveFromCollection(int indexId, CancellationToken cancellationToken = default)
    {
        ValidateId(indexId, nameof(indexId), "Index");
        await HttpService
             .Send(HttpMethod.Delete, $"/v0/indices/{indexId}/collect", AuthenticationMode.Required,
                   cancellationToken : cancellationToken).ConfigureAwait(false);
    }

    private static string? FormatEnum<TEnum>(TEnum? value) where TEnum : struct, Enum =>
        value.HasValue
            ? Convert.ToInt32(value.Value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture)
            : null;

    private static string? FormatInteger(int? value) => value?.ToString(CultureInfo.InvariantCulture);

    private static void ValidateSubjectType(SubjectType? type)
    {
        if (type.HasValue && !Enum.IsDefined(type.Value))
        {
            throw new ArgumentOutOfRangeException(nameof(type), type, "Subject type is invalid.");
        }
    }

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
