using System.Collections.Concurrent;

namespace Banned.Bangumi.Test;

internal sealed class TestHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _responseFactory;

    internal TestHttpMessageHandler(
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> responseFactory)
    {
        _responseFactory = responseFactory;
    }

    internal ConcurrentQueue<CapturedRequest> Requests { get; } = new();

    internal bool IsDisposed { get; private set; }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        Requests.Enqueue(await CapturedRequest.Create(request, cancellationToken).ConfigureAwait(false));
        return await _responseFactory(request, cancellationToken).ConfigureAwait(false);
    }

    protected override void Dispose(bool disposing)
    {
        IsDisposed = true;
        base.Dispose(disposing);
    }
}

internal sealed record CapturedRequest(
    HttpMethod Method,
    Uri Uri,
    string? UserAgent,
    string? Authorization,
    string? ContentType,
    string? Body)
{
    internal static async Task<CapturedRequest> Create(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var body = request.Content is null
            ? null
            : await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        return new CapturedRequest(
            request.Method,
            request.RequestUri!,
            request.Headers.UserAgent.ToString(),
            request.Headers.Authorization?.ToString(),
            request.Content?.Headers.ContentType?.ToString(),
            body);
    }
}
