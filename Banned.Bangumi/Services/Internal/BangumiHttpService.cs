using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Banned.Bangumi.Exceptions;
using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Enums;

namespace Banned.Bangumi.Services.Internal;

internal sealed class BangumiHttpService : IDisposable
{
    private const int MaximumDiagnosticBodyLength = 16 * 1024;

    private readonly string? _accessToken;
    private readonly Uri _baseAddress;
    private readonly bool _disposeHttpClient;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly TimeSpan _timeout;
    private readonly string _userAgent;
    private bool _disposed;

    internal BangumiHttpService(BangumiClientOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        ValidateOptions(options);

        _baseAddress = options.BaseAddress;
        _userAgent = options.UserAgent;
        _accessToken = string.IsNullOrWhiteSpace(options.AccessToken) ? null : options.AccessToken;
        _timeout = options.Timeout;
        _disposeHttpClient = options.HttpClient is null;
        _httpClient = options.HttpClient ?? new HttpClient(new HttpClientHandler
        {
            AllowAutoRedirect = false
        })
        {
            Timeout = Timeout.InfiniteTimeSpan
        };
        _serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        _serializerOptions.Converters.Add(
            new JsonStringEnumConverter<PersonCareer>(JsonNamingPolicy.CamelCase));
        _serializerOptions.Converters.Add(
            new JsonStringEnumConverter<SubjectSearchSort>(JsonNamingPolicy.CamelCase));
    }

    internal bool OwnsHttpClient => _disposeHttpClient;

    internal static string AddQueryString(
        string path,
        IEnumerable<KeyValuePair<string, string?>> parameters)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentNullException.ThrowIfNull(parameters);

        var builder = new StringBuilder(path);
        var separator = path.Contains('?') ? '&' : '?';

        foreach (var parameter in parameters)
        {
            if (parameter.Value is null)
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(parameter.Key))
            {
                throw new ArgumentException("Query parameter names must be non-empty.", nameof(parameters));
            }

            builder.Append(separator);
            builder.Append(Uri.EscapeDataString(parameter.Key));
            builder.Append('=');
            builder.Append(Uri.EscapeDataString(parameter.Value));
            separator = '&';
        }

        return builder.ToString();
    }

    internal Task<TResponse> Get<TResponse>(
        string path,
        AuthenticationMode authenticationMode = AuthenticationMode.None,
        CancellationToken cancellationToken = default) =>
        Send<TResponse>(HttpMethod.Get, path, authenticationMode, null, cancellationToken);

    internal async Task<Uri> GetRedirectUri(
        string path,
        AuthenticationMode authenticationMode = AuthenticationMode.None,
        CancellationToken cancellationToken = default)
    {
        using var timeoutSource = CreateTimeoutSource(cancellationToken);
        var effectiveCancellationToken = timeoutSource?.Token ?? cancellationToken;
        var requestUri = CreateRequestUri(path);
        using var response = await SendCore(
            HttpMethod.Get,
            path,
            authenticationMode,
            null,
            effectiveCancellationToken,
            allowRedirectResponse: true).ConfigureAwait(false);

        if (IsRedirectStatusCode(response.StatusCode) && response.Headers.Location is { } location)
        {
            return location.IsAbsoluteUri ? location : new Uri(requestUri, location);
        }

        var finalRequestUri = response.RequestMessage?.RequestUri;
        if (response.IsSuccessStatusCode &&
            finalRequestUri is not null &&
            finalRequestUri != requestUri)
        {
            return finalRequestUri;
        }

        throw new BangumiApiException(
            "Bangumi returned an image response without a redirect target.",
            response.StatusCode);
    }

    internal async Task<TResponse> Send<TResponse>(
        HttpMethod method,
        string path,
        AuthenticationMode authenticationMode,
        object? body = null,
        CancellationToken cancellationToken = default)
    {
        using var timeoutSource = CreateTimeoutSource(cancellationToken);
        var effectiveCancellationToken = timeoutSource?.Token ?? cancellationToken;
        using var response = await SendCore(
            method,
            path,
            authenticationMode,
            body,
            effectiveCancellationToken)
            .ConfigureAwait(false);
        var responseBody = await response.Content.ReadAsStringAsync(effectiveCancellationToken)
            .ConfigureAwait(false);

        if (string.IsNullOrWhiteSpace(responseBody))
        {
            throw new BangumiApiException(
                "Bangumi returned an empty response body.",
                response.StatusCode);
        }

        try
        {
            var result = JsonSerializer.Deserialize<TResponse>(responseBody, _serializerOptions);
            return result ?? throw new JsonException("The JSON response was null.");
        }
        catch (JsonException exception)
        {
            throw new BangumiApiException(
                "Bangumi returned a response that could not be deserialized.",
                response.StatusCode,
                responseBody: BoundDiagnosticBody(responseBody),
                innerException: exception);
        }
    }

    internal async Task Send(
        HttpMethod method,
        string path,
        AuthenticationMode authenticationMode,
        object? body = null,
        CancellationToken cancellationToken = default)
    {
        using var timeoutSource = CreateTimeoutSource(cancellationToken);
        var effectiveCancellationToken = timeoutSource?.Token ?? cancellationToken;
        using var response = await SendCore(
            method,
            path,
            authenticationMode,
            body,
            effectiveCancellationToken)
            .ConfigureAwait(false);
    }

    private async Task<HttpResponseMessage> SendCore(
        HttpMethod method,
        string path,
        AuthenticationMode authenticationMode,
        object? body,
        CancellationToken cancellationToken,
        bool allowRedirectResponse = false)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(method);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        if (authenticationMode == AuthenticationMode.Required && _accessToken is null)
        {
            throw new BangumiAuthenticationException(
                "This Bangumi API operation requires an access token.");
        }

        using var request = new HttpRequestMessage(method, CreateRequestUri(path));
        request.Headers.TryAddWithoutValidation("User-Agent", _userAgent);

        if (authenticationMode != AuthenticationMode.None && _accessToken is not null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }

        if (body is not null)
        {
            var json = JsonSerializer.Serialize(body, _serializerOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        var response = await _httpClient.SendAsync(
            request,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken).ConfigureAwait(false);

        if (response.IsSuccessStatusCode ||
            (allowRedirectResponse && IsRedirectStatusCode(response.StatusCode)))
        {
            return response;
        }

        try
        {
            await ThrowApiException(response, cancellationToken).ConfigureAwait(false);
            throw new InvalidOperationException("The API error handler returned unexpectedly.");
        }
        finally
        {
            response.Dispose();
        }
    }

    private async Task ThrowApiException(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var boundedBody = BoundDiagnosticBody(responseBody);
        ErrorDetail? error = null;

        if (!string.IsNullOrWhiteSpace(responseBody))
        {
            try
            {
                error = JsonSerializer.Deserialize<ErrorDetail>(responseBody, _serializerOptions);
            }
            catch (JsonException)
            {
                // The raw, bounded body remains available for diagnostics.
            }
        }

        var message = error?.Description ?? error?.Title ??
            $"Bangumi returned HTTP {(int)response.StatusCode} ({response.ReasonPhrase}).";

        if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
        {
            throw new BangumiAuthenticationException(
                message,
                response.StatusCode,
                error,
                boundedBody);
        }

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            throw new BangumiRateLimitException(
                message,
                response.StatusCode,
                error,
                boundedBody,
                GetRetryAfter(response.Headers.RetryAfter));
        }

        throw new BangumiApiException(message, response.StatusCode, error, boundedBody);
    }

    private Uri CreateRequestUri(string path)
    {
        var baseAddress = _baseAddress.AbsoluteUri.TrimEnd('/');
        return new Uri($"{baseAddress}/{path.TrimStart('/')}", UriKind.Absolute);
    }

    private CancellationTokenSource? CreateTimeoutSource(CancellationToken cancellationToken)
    {
        if (_timeout == Timeout.InfiniteTimeSpan)
        {
            return null;
        }

        var source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        source.CancelAfter(_timeout);
        return source;
    }

    private static TimeSpan? GetRetryAfter(RetryConditionHeaderValue? retryAfter)
    {
        if (retryAfter?.Delta is { } delta)
        {
            return delta;
        }

        if (retryAfter?.Date is { } date)
        {
            var remaining = date - DateTimeOffset.UtcNow;
            return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
        }

        return null;
    }

    private static bool IsRedirectStatusCode(HttpStatusCode statusCode) =>
        statusCode is HttpStatusCode.MultipleChoices or
            HttpStatusCode.MovedPermanently or
            HttpStatusCode.Found or
            HttpStatusCode.SeeOther or
            HttpStatusCode.TemporaryRedirect or
            HttpStatusCode.PermanentRedirect;

    private static string? BoundDiagnosticBody(string responseBody) =>
        string.IsNullOrEmpty(responseBody)
            ? null
            : responseBody[..Math.Min(responseBody.Length, MaximumDiagnosticBodyLength)];

    private static void ValidateOptions(BangumiClientOptions options)
    {
        if (options.BaseAddress is null ||
            !options.BaseAddress.IsAbsoluteUri ||
            (options.BaseAddress.Scheme != Uri.UriSchemeHttp &&
             options.BaseAddress.Scheme != Uri.UriSchemeHttps))
        {
            throw new ArgumentException(
                "BaseAddress must be an absolute HTTP or HTTPS URI.",
                nameof(options));
        }

        if (string.IsNullOrWhiteSpace(options.UserAgent) || ContainsNewLine(options.UserAgent))
        {
            throw new ArgumentException(
                "UserAgent must be non-empty and cannot contain line breaks.",
                nameof(options));
        }

        if (options.AccessToken is not null && ContainsNewLine(options.AccessToken))
        {
            throw new ArgumentException(
                "AccessToken cannot contain line breaks.",
                nameof(options));
        }

        if (options.Timeout != Timeout.InfiniteTimeSpan && options.Timeout <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                "Timeout must be positive or Timeout.InfiniteTimeSpan.");
        }
    }

    private static bool ContainsNewLine(string value) =>
        value.Contains('\r') || value.Contains('\n');

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        if (_disposeHttpClient)
        {
            _httpClient.Dispose();
        }
    }
}
