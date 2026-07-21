using Banned.Bangumi;
using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Subjects;
using System.Net;
using System.Text;

var handler = new SmokeHttpMessageHandler();
using var httpClient = new HttpClient(handler);
using var client = new BangumiClient(new BangumiClientOptions
{
    UserAgent  = "Banned.Bangumi.AotSmoke/1.0",
    HttpClient = httpClient
});

var calendar = await client.Calendar.Get();
if (calendar.Count != 1 || calendar[0].Weekday?.Id != 1)
{
    throw new InvalidOperationException("Calendar response deserialization failed.");
}

var subjects = await client.Subjects.Search(new SubjectSearchRequest
{
    Keyword = "AOT",
    Sort    = SubjectSearchSort.Rank,
    Filter = new SubjectSearchFilter
    {
        NsfwFilter = NsfwFilterMode.Exclude
    },
    Limit = 20
});

if (subjects.Total != 0 || handler.SearchRequestBody is null ||
    !handler.SearchRequestBody.Contains("\"keyword\":\"AOT\"", StringComparison.Ordinal) ||
    !handler.SearchRequestBody.Contains("\"nsfw\":false", StringComparison.Ordinal))
{
    throw new InvalidOperationException("Search request serialization or response deserialization failed.");
}

internal sealed class SmokeHttpMessageHandler : HttpMessageHandler
{
    internal string? SearchRequestBody { get; private set; }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                  CancellationToken cancellationToken)
    {
        string responseBody;
        if (request.RequestUri?.AbsolutePath == "/calendar")
        {
            responseBody = """
                           [{"weekday":{"en":"Monday","cn":"星期一","ja":"月曜日","id":1},"items":[]}]
                           """;
        }
        else if (request.RequestUri?.AbsolutePath == "/v0/search/subjects")
        {
            SearchRequestBody = await request.Content!.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            responseBody = "{\"total\":0,\"limit\":20,\"offset\":0,\"data\":[]}";
        }
        else
        {
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(responseBody, Encoding.UTF8, "application/json")
        };
    }
}
