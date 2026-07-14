using Banned.Bangumi.Exceptions;
using Banned.Bangumi.Models.Characters;
using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Enums;
using Banned.Bangumi.Services.Internal;
using System.Globalization;

namespace Banned.Bangumi.Services;

/// <summary>
/// 提供 Bangumi 角色操作。<br/>
/// Provides operations for Bangumi characters.
/// </summary>
public sealed class CharacterService
{
    internal CharacterService(BangumiHttpService httpService) => HttpService = httpService;

    internal BangumiHttpService HttpService { get; }

    /// <summary>
    /// 获取指定角色。<br/>
    /// Gets the specified character.
    /// </summary>
    /// <param name="characterId">角色 ID。 / Character ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>角色详情。<br/>The character details.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="characterId"/> 小于 1。 / <paramref name="characterId"/> is less than 1.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<Character> Get(int characterId, CancellationToken cancellationToken = default)
    {
        ValidateCharacterId(characterId);
        return await HttpService
                    .Get<Character>($"/v0/characters/{characterId}", AuthenticationMode.None, cancellationToken)
                    .ConfigureAwait(false);
    }

    /// <summary>
    /// 获取指定尺寸角色图片的重定向目标。<br/>
    /// Gets the redirect target for a character image of the specified size.
    /// </summary>
    /// <param name="characterId">角色 ID。 / Character ID.</param>
    /// <param name="size">图片尺寸。 / Image size.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>图片地址。<br/>The image URI.</returns>
    /// <exception cref="ArgumentOutOfRangeException">角色 ID 或图片尺寸无效。 / The character ID or image size is invalid.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或未返回重定向目标。 / The API returns an error or does not return a redirect target.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<Uri> GetImageUri(int               characterId, CharacterImageSize size,
                                       CancellationToken cancellationToken = default)
    {
        ValidateCharacterId(characterId);
        if (!Enum.IsDefined(size))
        {
            throw new ArgumentOutOfRangeException(nameof(size), size, "The image size is invalid.");
        }

        var path = BangumiHttpService.AddQueryString($"/v0/characters/{characterId}/image",
                                                     new Dictionary<string, string?>
                                                         { ["type"] = size.ToString().ToLowerInvariant() });

        return await HttpService.GetRedirectUri(path, AuthenticationMode.Optional, cancellationToken)
                                .ConfigureAwait(false);
    }

    /// <summary>
    /// 获取指定角色的关联条目。<br/>
    /// Gets subjects related to the specified character.
    /// </summary>
    /// <param name="characterId">角色 ID。 / Character ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>关联条目列表。<br/>The related subjects.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="characterId"/> 小于 1。 / <paramref name="characterId"/> is less than 1.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<IReadOnlyList<RelatedSubject>> GetSubjects(int               characterId,
                                                                 CancellationToken cancellationToken = default)
    {
        ValidateCharacterId(characterId);
        return await HttpService
                    .Get<IReadOnlyList<RelatedSubject>>($"/v0/characters/{characterId}/subjects",
                                                        AuthenticationMode.None, cancellationToken)
                    .ConfigureAwait(false);
    }

    /// <summary>
    /// 获取指定角色的关联人物及其配套条目信息。<br/>
    /// Gets persons related to the specified character together with associated subject information.
    /// </summary>
    /// <param name="characterId">角色 ID。 / Character ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>关联人物列表。<br/>The related persons.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="characterId"/> 小于 1。 / <paramref name="characterId"/> is less than 1.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<IReadOnlyList<CharacterRelatedPerson>> GetPersons(int               characterId,
                                                                        CancellationToken cancellationToken = default)
    {
        ValidateCharacterId(characterId);
        return await HttpService
                    .Get<IReadOnlyList<CharacterRelatedPerson>>($"/v0/characters/{characterId}/persons",
                                                                AuthenticationMode.None, cancellationToken)
                    .ConfigureAwait(false);
    }

    /// <summary>
    /// 搜索角色。<br/>
    /// Searches characters.
    /// </summary>
    /// <param name="request">搜索请求。 / Search request.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>分页搜索结果。<br/>A page of search results.</returns>
    /// <exception cref="ArgumentException">搜索关键字为空，或分页值无效。 / The search keyword is empty or a pagination value is invalid.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="request"/> 为 <see langword="null"/>。 / <paramref name="request"/> is <see langword="null"/>.</exception>
    /// <exception cref="BangumiApiException">API 返回错误或响应无法解析。 / The API returns an error or the response cannot be parsed.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task<PagedResult<Character>> Search(CharacterSearchRequest request,
                                                     CancellationToken      cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ValidateSearchRequest(request);

        var path = BangumiHttpService.AddQueryString("/v0/search/characters",
                                                     new Dictionary<string, string?>
                                                     {
                                                         ["limit"]  = FormatInteger(request.Limit),
                                                         ["offset"] = FormatInteger(request.Offset)
                                                     });

        return await HttpService
                    .Send<PagedResult<Character>>(HttpMethod.Post, path, AuthenticationMode.None, request,
                                                  cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 为当前用户收藏指定角色。<br/>
    /// Adds the specified character to the current user's collection.
    /// </summary>
    /// <param name="characterId">角色 ID。 / Character ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>表示异步操作的任务。<br/>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="characterId"/> 小于 1。 / <paramref name="characterId"/> is less than 1.</exception>
    /// <exception cref="BangumiAuthenticationException">未配置访问令牌或 API 拒绝鉴权。 / No access token is configured or the API rejects authentication.</exception>
    /// <exception cref="BangumiApiException">API 返回错误。 / The API returns an error.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task AddToCollection(int characterId, CancellationToken cancellationToken = default)
    {
        ValidateCharacterId(characterId);
        await HttpService
             .Send(HttpMethod.Post, $"/v0/characters/{characterId}/collect", AuthenticationMode.Required,
                   cancellationToken : cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 为当前用户取消收藏指定角色。<br/>
    /// Removes the specified character from the current user's collection.
    /// </summary>
    /// <param name="characterId">角色 ID。 / Character ID.</param>
    /// <param name="cancellationToken">用于取消请求的令牌。 / Token used to cancel the request.</param>
    /// <returns>表示异步操作的任务。<br/>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="characterId"/> 小于 1。 / <paramref name="characterId"/> is less than 1.</exception>
    /// <exception cref="BangumiAuthenticationException">未配置访问令牌或 API 拒绝鉴权。 / No access token is configured or the API rejects authentication.</exception>
    /// <exception cref="BangumiApiException">API 返回错误。 / The API returns an error.</exception>
    /// <exception cref="OperationCanceledException">请求被取消或超时。 / The request is cancelled or times out.</exception>
    public async Task RemoveFromCollection(int characterId, CancellationToken cancellationToken = default)
    {
        ValidateCharacterId(characterId);
        await HttpService.Send(HttpMethod.Delete, $"/v0/characters/{characterId}/collect",
                               AuthenticationMode.Required, cancellationToken : cancellationToken)
                         .ConfigureAwait(false);
    }

    private static string? FormatInteger(int? value) => value?.ToString(CultureInfo.InvariantCulture);

    private static void ValidateSearchRequest(CharacterSearchRequest request)
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

    private static void ValidateCharacterId(int characterId)
    {
        if (characterId < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(characterId), characterId, "Character ID must be positive.");
        }
    }
}
