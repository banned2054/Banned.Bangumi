using Banned.Bangumi.Exceptions;
using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Enums;
using Banned.Bangumi.Models.Subjects;
using Banned.Bangumi.Services.Internal;
using System.Globalization;

namespace Banned.Bangumi.Services;

/// <summary>
/// 提供 Bangumi 条目操作。<br/>
/// Provides operations for Bangumi subjects.
/// </summary>
public sealed class SubjectService
{
    internal SubjectService(BangumiHttpService httpService) => HttpService = httpService;

    internal BangumiHttpService HttpService { get; }

    /// <summary>
    /// 按条件浏览条目。<br/>
    /// Browses subjects using the specified criteria.
    /// </summary>
    /// <param name="request">浏览条件。 / Browse criteria.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>分页条目结果。<br/>A page of subjects.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="request"/> 为 <see langword="null"/>。 / <paramref name="request"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">请求包含无效的类型或分页值。 / The request contains an invalid type or pagination value.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<PagedResult<Subject>> Browse(SubjectBrowseRequest request,
                                                   CancellationToken    cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ValidateBrowseRequest(request);

        var path = BangumiHttpService.AddQueryString("/v0/subjects", new Dictionary<string, string?>
        {
            ["type"] = ((int)request.Type).ToString(CultureInfo.InvariantCulture),
            ["cat"] = request.Category is { } category
                ? ((int)category).ToString(CultureInfo.InvariantCulture)
                : null,
            ["series"]   = FormatBoolean(request.Series),
            ["platform"] = request.Platform,
            ["sort"]     = request.Sort?.ToString().ToLowerInvariant(),
            ["year"]     = FormatInteger(request.Year),
            ["month"]    = FormatInteger(request.Month),
            ["limit"]    = FormatInteger(request.Limit),
            ["offset"]   = FormatInteger(request.Offset)
        });

        return await HttpService.Get<PagedResult<Subject>>(path, AuthenticationMode.Optional, cancellationToken)
                                .ConfigureAwait(false);
    }

    /// <summary>
    /// 获取指定条目。<br/>
    /// Gets the specified subject.
    /// </summary>
    /// <param name="subjectId">条目 ID。 / Subject ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>条目详情。<br/>The subject details.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="subjectId"/> 小于 1。 / <paramref name="subjectId"/> is less than 1.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<Subject> Get(int subjectId, CancellationToken cancellationToken = default)
    {
        ValidateSubjectId(subjectId);
        return await HttpService
                    .Get<Subject>($"/v0/subjects/{subjectId}", AuthenticationMode.Optional, cancellationToken)
                    .ConfigureAwait(false);
    }

    /// <summary>
    /// 获取指定尺寸条目图片的重定向目标。<br/>
    /// Gets the redirect target for a subject image of the specified size.
    /// </summary>
    /// <param name="subjectId">条目 ID。 / Subject ID.</param>
    /// <param name="size">图片尺寸。 / Image size.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>图片地址。<br/>The image URI.</returns>
    /// <exception cref="ArgumentOutOfRangeException">条目 ID 或图片尺寸无效。 / The subject ID or image size is invalid.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或未返回重定向目标。 / The API returns an error or does not return a redirect target.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<Uri> GetImageUri(int               subjectId, SubjectImageSize size,
                                       CancellationToken cancellationToken = default)
    {
        ValidateSubjectId(subjectId);
        if (!Enum.IsDefined(size))
        {
            throw new ArgumentOutOfRangeException(nameof(size), size, "The image size is invalid.");
        }

        var path = BangumiHttpService.AddQueryString($"/v0/subjects/{subjectId}/image",
                                                     new Dictionary<string, string?>
                                                         { ["type"] = size.ToString().ToLowerInvariant() });

        return await HttpService.GetRedirectUri(path, AuthenticationMode.Optional, cancellationToken)
                                .ConfigureAwait(false);
    }

    /// <summary>
    /// 获取参与指定条目制作的人物。<br/>
    /// Gets persons involved with the specified subject.
    /// </summary>
    /// <param name="subjectId">条目 ID。 / Subject ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>关联人物列表。<br/>The related persons.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="subjectId"/> 小于 1。 / <paramref name="subjectId"/> is less than 1.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<IReadOnlyList<RelatedPerson>> GetPersons(int               subjectId,
                                                               CancellationToken cancellationToken = default)
    {
        ValidateSubjectId(subjectId);
        return await HttpService
                    .Get<IReadOnlyList<RelatedPerson>>($"/v0/subjects/{subjectId}/persons", AuthenticationMode.Optional,
                                                       cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 获取指定条目的关联角色。<br/>
    /// Gets characters related to the specified subject.
    /// </summary>
    /// <param name="subjectId">条目 ID。 / Subject ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>关联角色列表。<br/>The related characters.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="subjectId"/> 小于 1。 / <paramref name="subjectId"/> is less than 1.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<IReadOnlyList<RelatedCharacter>> GetCharacters(int               subjectId,
                                                                     CancellationToken cancellationToken = default)
    {
        ValidateSubjectId(subjectId);
        return await HttpService
                    .Get<IReadOnlyList<RelatedCharacter>>($"/v0/subjects/{subjectId}/characters",
                                                          AuthenticationMode.Optional, cancellationToken)
                    .ConfigureAwait(false);
    }

    /// <summary>
    /// 获取指定条目的关联条目。<br/>
    /// Gets subjects related to the specified subject.
    /// </summary>
    /// <param name="subjectId">条目 ID。 / Subject ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>关联条目列表。<br/>The related subjects.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="subjectId"/> 小于 1。 / <paramref name="subjectId"/> is less than 1.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<IReadOnlyList<SubjectRelation>> GetRelations(int               subjectId,
                                                                   CancellationToken cancellationToken = default)
    {
        ValidateSubjectId(subjectId);
        return await HttpService
                    .Get<IReadOnlyList<SubjectRelation>>($"/v0/subjects/{subjectId}/subjects",
                                                         AuthenticationMode.Optional, cancellationToken)
                    .ConfigureAwait(false);
    }

    /// <summary>
    /// 搜索条目。<br/>
    /// Searches subjects.
    /// </summary>
    /// <param name="request">搜索请求。 / Search request.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>分页搜索结果。<br/>A page of search results.</returns>
    /// <exception cref="ArgumentException">搜索关键字为空，或分页值无效。 / The search keyword is empty or a pagination value is invalid.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="request"/> 为 <see langword="null"/>。 / <paramref name="request"/> is <see langword="null"/>.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<PagedResult<Subject>> Search(SubjectSearchRequest request,
                                                   CancellationToken    cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ValidateSearchRequest(request);

        var path = BangumiHttpService.AddQueryString("/v0/search/subjects",
                                                     new Dictionary<string, string?>
                                                     {
                                                         ["limit"]  = FormatInteger(request.Limit),
                                                         ["offset"] = FormatInteger(request.Offset)
                                                     });

        return await HttpService
                    .Send<PagedResult<Subject>>(HttpMethod.Post, path, AuthenticationMode.None, request,
                                                cancellationToken).ConfigureAwait(false);
    }

    private static string? FormatBoolean(bool? value) => value?.ToString().ToLowerInvariant();

    private static string? FormatInteger(int? value) => value?.ToString(CultureInfo.InvariantCulture);

    private static void ValidateBrowseRequest(SubjectBrowseRequest request)
    {
        if (!Enum.IsDefined(request.Type))
        {
            throw new ArgumentOutOfRangeException(nameof(request), request.Type, "The subject type is invalid.");
        }

        if (request.Category is { } category && !Enum.IsDefined(category))
        {
            throw new ArgumentOutOfRangeException(nameof(request), category, "The subject category is invalid.");
        }

        if (request.Sort is { } sort && !Enum.IsDefined(sort))
        {
            throw new ArgumentOutOfRangeException(nameof(request), sort, "The browse sort order is invalid.");
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

    private static void ValidateSearchRequest(SubjectSearchRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Keyword))
        {
            throw new ArgumentException("Keyword must be non-empty.", nameof(request));
        }

        if (!Enum.IsDefined(request.Sort))
        {
            throw new ArgumentOutOfRangeException(nameof(request), request.Sort, "The search sort order is invalid.");
        }

        if (request.Limit is < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(request), request.Limit, "Limit must be positive.");
        }

        if (request.Offset is < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request), request.Offset, "Offset cannot be negative.");
        }
    }

    private static void ValidateSubjectId(int subjectId)
    {
        if (subjectId < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(subjectId), subjectId, "Subject ID must be positive.");
        }
    }
}
