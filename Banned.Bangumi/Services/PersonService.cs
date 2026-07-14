using Banned.Bangumi.Exceptions;
using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Persons;
using Banned.Bangumi.Services.Internal;
using System.Globalization;

namespace Banned.Bangumi.Services;

/// <summary>
/// 提供 Bangumi 人物操作。<br/>
/// Provides operations for Bangumi persons.
/// </summary>
public sealed class PersonService
{
    internal PersonService(BangumiHttpService httpService) => HttpService = httpService;

    internal BangumiHttpService HttpService { get; }

    /// <summary>
    /// 获取指定人物。<br/>
    /// Gets the specified person.
    /// </summary>
    /// <param name="personId">人物 ID。 / Person ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>人物详情。<br/>The person details.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="personId"/> 小于 1。 / <paramref name="personId"/> is less than 1.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<PersonDetail> Get(int personId, CancellationToken cancellationToken = default)
    {
        ValidatePersonId(personId);
        return await HttpService
                    .Get<PersonDetail>($"/v0/persons/{personId}", AuthenticationMode.None, cancellationToken)
                    .ConfigureAwait(false);
    }

    /// <summary>
    /// 获取指定尺寸人物图片的重定向目标。<br/>
    /// Gets the redirect target for a person image of the specified size.
    /// </summary>
    /// <param name="personId">人物 ID。 / Person ID.</param>
    /// <param name="size">图片尺寸。 / Image size.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>图片地址。<br/>The image URI.</returns>
    /// <exception cref="ArgumentOutOfRangeException">人物 ID 或图片尺寸无效。 / The person ID or image size is invalid.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或未返回重定向目标。 / The API returns an error or does not return a redirect target.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<Uri> GetImageUri(int personId, ImageSize size, CancellationToken cancellationToken = default)
    {
        ValidatePersonId(personId);
        if (!Enum.IsDefined(size) || size == ImageSize.Common)
        {
            throw new ArgumentOutOfRangeException(nameof(size), size, "The image size is invalid.");
        }

        var path = BangumiHttpService.AddQueryString($"/v0/persons/{personId}/image",
                                                     new Dictionary<string, string?>
                                                         { ["type"] = size.ToString().ToLowerInvariant() });

        return await HttpService.GetRedirectUri(path, AuthenticationMode.Optional, cancellationToken)
                                .ConfigureAwait(false);
    }

    /// <summary>
    /// 获取指定人物的关联条目。<br/>
    /// Gets subjects related to the specified person.
    /// </summary>
    /// <param name="personId">人物 ID。 / Person ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>关联条目列表。<br/>The related subjects.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="personId"/> 小于 1。 / <paramref name="personId"/> is less than 1.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<IReadOnlyList<RelatedSubject>> GetSubjects(int               personId,
                                                                 CancellationToken cancellationToken = default)
    {
        ValidatePersonId(personId);
        return await HttpService
                    .Get<IReadOnlyList<RelatedSubject>>($"/v0/persons/{personId}/subjects", AuthenticationMode.None,
                                                        cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 获取指定人物的关联角色及其配套条目信息。<br/>
    /// Gets characters related to the specified person together with associated subject information.
    /// </summary>
    /// <param name="personId">人物 ID。 / Person ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>关联角色列表。<br/>The related characters.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="personId"/> 小于 1。 / <paramref name="personId"/> is less than 1.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<IReadOnlyList<PersonRelatedCharacter>> GetCharacters(
        int personId, CancellationToken cancellationToken = default)
    {
        ValidatePersonId(personId);
        return await HttpService
                    .Get<IReadOnlyList<PersonRelatedCharacter>>($"/v0/persons/{personId}/characters",
                                                                AuthenticationMode.None, cancellationToken)
                    .ConfigureAwait(false);
    }

    /// <summary>
    /// 搜索人物。<br/>
    /// Searches persons.
    /// </summary>
    /// <param name="request">搜索请求。 / Search request.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>分页搜索结果。<br/>A page of search results.</returns>
    /// <exception cref="ArgumentException">搜索关键字为空，或分页值无效。 / The search keyword is empty or a pagination value is invalid.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="request"/> 为 <see langword="null"/>。 / <paramref name="request"/> is <see langword="null"/>.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<PagedResult<Person>> Search(PersonSearchRequest request,
                                                  CancellationToken   cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ValidateSearchRequest(request);

        var path = BangumiHttpService.AddQueryString("/v0/search/persons",
                                                     new Dictionary<string, string?>
                                                     {
                                                         ["limit"]  = FormatInteger(request.Limit),
                                                         ["offset"] = FormatInteger(request.Offset)
                                                     });

        return await HttpService
                    .Send<PagedResult<Person>>(HttpMethod.Post, path, AuthenticationMode.None, request,
                                               cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 为当前用户收藏指定人物。<br/>
    /// Adds the specified person to the current user's collection.
    /// </summary>
    /// <param name="personId">人物 ID。 / Person ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>表示异步操作的任务。<br/>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="personId"/> 小于 1。 / <paramref name="personId"/> is less than 1.</exception>
    /// <exception cref="BangumiAuthenticationException">未配置访问令牌或 API 拒绝鉴权。 / No access token is configured or the API rejects authentication.</exception>
    /// <exception cref="BangumiApiException">API 返回错误。 / The API returns an error.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task AddToCollection(int personId, CancellationToken cancellationToken = default)
    {
        ValidatePersonId(personId);
        await HttpService
             .Send(HttpMethod.Post, $"/v0/persons/{personId}/collect", AuthenticationMode.Required,
                   cancellationToken : cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 为当前用户取消收藏指定人物。<br/>
    /// Removes the specified person from the current user's collection.
    /// </summary>
    /// <param name="personId">人物 ID。 / Person ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>表示异步操作的任务。<br/>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="personId"/> 小于 1。 / <paramref name="personId"/> is less than 1.</exception>
    /// <exception cref="BangumiAuthenticationException">未配置访问令牌或 API 拒绝鉴权。 / No access token is configured or the API rejects authentication.</exception>
    /// <exception cref="BangumiApiException">API 返回错误。 / The API returns an error.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task RemoveFromCollection(int personId, CancellationToken cancellationToken = default)
    {
        ValidatePersonId(personId);
        await HttpService.Send(HttpMethod.Delete, $"/v0/persons/{personId}/collect",
                               AuthenticationMode.Required, cancellationToken : cancellationToken)
                         .ConfigureAwait(false);
    }

    private static string? FormatInteger(int? value) => value?.ToString(CultureInfo.InvariantCulture);

    private static void ValidateSearchRequest(PersonSearchRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Keyword))
        {
            throw new ArgumentException("Keyword must be non-empty.", nameof(request));
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

    private static void ValidatePersonId(int personId)
    {
        if (personId < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(personId), personId, "Person ID must be positive.");
        }
    }
}
