using Banned.Bangumi.Models.Calendar;
using Banned.Bangumi.Models.Subjects;
using System.Text.Json;

namespace Banned.Bangumi.Test;

[TestFixture]
public sealed class CalendarModelTests
{
    [Test]
    public void CalendarDay_DeserializesLegacyCalendarStructure()
    {
        const string json = """
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
                    "url": "https://bgm.tv/subject/12",
                    "type": 2,
                    "name": "ちょびっツ",
                    "name_cn": "人形电脑天使心",
                    "eps": 27,
                    "eps_count": 27,
                    "images": {
                      "large": "https://example.test/large.jpg"
                    },
                    "rating": {
                      "total": 2289,
                      "count": { "1": 5, "10": 130 },
                      "score": 7.6
                    },
                    "rank": 573,
                    "collection": {
                      "wish": 608,
                      "collect": 3010,
                      "doing": 103,
                      "on_hold": 284,
                      "dropped": 86
                    },
                    "future_field": true
                  }
                ]
              }
            ]
            """;

        var calendar = JsonSerializer.Deserialize<IReadOnlyList<CalendarDay>>(json);
        var day      = calendar!.Single();
        var subject  = day.Items.Single();

        Assert.Multiple(() =>
        {
            Assert.That(day.Weekday?.English, Is.EqualTo("Mon"));
            Assert.That(day.Weekday?.Chinese, Is.EqualTo("星期一"));
            Assert.That(day.Weekday?.Japanese, Is.EqualTo("月曜日"));
            Assert.That(day.Weekday?.Id, Is.EqualTo(1));
            Assert.That(subject.Id, Is.EqualTo(12));
            Assert.That(subject.Type, Is.EqualTo(LegacySubjectType.Anime));
            Assert.That(subject.NameCn, Is.EqualTo("人形电脑天使心"));
            Assert.That(subject.Eps, Is.EqualTo(27));
            Assert.That(subject.EpsCount, Is.EqualTo(27));
            Assert.That(subject.Images?.Large, Is.EqualTo("https://example.test/large.jpg"));
            Assert.That(subject.Rating?.Total, Is.EqualTo(2289));
            Assert.That(subject.Rating?.Count?.Score1, Is.EqualTo(5));
            Assert.That(subject.Rating?.Count?.Score10, Is.EqualTo(130));
            Assert.That(subject.Rating?.Score, Is.EqualTo(7.6));
            Assert.That(subject.Collection?.OnHold, Is.EqualTo(284));
        });
    }

    [Test]
    public void CalendarDay_UsesEmptyItemsWhenJsonOmitsCollection()
    {
        var day = JsonSerializer.Deserialize<CalendarDay>("{\"weekday\":{\"id\":1}}");

        Assert.That(day!.Items, Is.Empty);
    }
}
