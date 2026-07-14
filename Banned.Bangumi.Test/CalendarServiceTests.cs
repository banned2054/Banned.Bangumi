using Banned.Bangumi.Models.Subjects;
using System.Net;
using System.Text;

namespace Banned.Bangumi.Test;

[TestFixture]
public sealed class CalendarServiceTests
{
    [Test]
    public async Task Get_SendsUnauthenticatedRequestAndDeserializesCalendar()
    {
        const string responseJson = """
            [
              {
                "weekday": {
                  "en": "Mon",
                  "cn": "星期一",
                  "ja": "月曜日",
                  "id": 1
                },
                "items": [
                  {
                    "id": 12,
                    "type": 2,
                    "name": "ちょびっツ",
                    "name_cn": "人形电脑天使心"
                  }
                ]
              }
            ]
            """;
        var handler = new TestHttpMessageHandler((_, _) => Task.FromResult(
                                                                           new HttpResponseMessage(HttpStatusCode.OK)
                                                                           {
                                                                               Content = new StringContent(responseJson,
                                                                                        Encoding.UTF8,
                                                                                        "application/json")
                                                                           }));
        using var httpClient = new HttpClient(handler);
        using var client = new BangumiClient(new BangumiClientOptions
        {
            BaseAddress = new Uri("https://api.example/root"),
            AccessToken = "must-not-be-sent",
            UserAgent   = "Banned.Bangumi.Test/1.0",
            HttpClient  = httpClient
        });

        var calendar = await client.Calendar.Get();

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        var day     = calendar.Single();
        var subject = day.Items.Single();
        Assert.Multiple(() =>
        {
            Assert.That(request!.Method, Is.EqualTo(HttpMethod.Get));
            Assert.That(request.Uri, Is.EqualTo(new Uri("https://api.example/root/calendar")));
            Assert.That(request.UserAgent, Is.EqualTo("Banned.Bangumi.Test/1.0"));
            Assert.That(request.Authorization, Is.Null);
            Assert.That(request.Body, Is.Null);
            Assert.That(day.Weekday?.Id, Is.EqualTo(1));
            Assert.That(day.Weekday?.Chinese, Is.EqualTo("星期一"));
            Assert.That(subject.Id, Is.EqualTo(12));
            Assert.That(subject.Type, Is.EqualTo(SubjectType.Anime));
            Assert.That(subject.NameCn, Is.EqualTo("人形电脑天使心"));
        });
    }

    [Test]
    public void Get_PropagatesCancellationToken()
    {
        var handler = new TestHttpMessageHandler(async (_, cancellationToken) =>
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            throw new InvalidOperationException("The request should have been cancelled.");
        });
        using var httpClient = new HttpClient(handler);
        using var client = new BangumiClient(new BangumiClientOptions
        {
            UserAgent  = "Banned.Bangumi.Test/1.0",
            HttpClient = httpClient
        });
        using var cancellationSource = new CancellationTokenSource();
        cancellationSource.Cancel();

        Assert.That(
                    async () => await client.Calendar.Get(cancellationSource.Token),
                    Throws.InstanceOf<OperationCanceledException>());
    }
}
