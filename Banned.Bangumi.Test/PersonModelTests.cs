using Banned.Bangumi.Models.Characters;
using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Persons;
using Banned.Bangumi.Models.Subjects;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Test;

[TestFixture]
public sealed class PersonModelTests
{
    private static readonly JsonSerializerOptions SerializerOptions = CreateSerializerOptions();

    [Test]
    public void PersonDetail_DeserializesDeclaredFields()
    {
        const string json = """
            {"id":7,"name":"Voice Actor","type":1,"career":["seiyu","actor"],"images":{"large":"large.jpg","medium":"medium.jpg","small":"small.jpg","grid":"grid.jpg"},"summary":"Summary","locked":false,"last_modified":"2024-01-02T03:04:05Z","infobox":[{"key":"简体中文名","value":"声优"}],"gender":"女","blood_type":3,"birth_year":1980,"birth_mon":1,"birth_day":2,"stat":{"comments":3,"collects":4}}
            """;

        var result = JsonSerializer.Deserialize<PersonDetail>(json, SerializerOptions);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result!.Id, Is.EqualTo(7));
            Assert.That(result.Type, Is.EqualTo(PersonType.Individual));
            Assert.That(result.Careers, Is.EqualTo(new[] { PersonCareer.Seiyu, PersonCareer.Actor }));
            Assert.That(result.Images!.Grid, Is.EqualTo("grid.jpg"));
            Assert.That(result.LastModified, Is.EqualTo(DateTimeOffset.Parse("2024-01-02T03:04:05Z")));
            Assert.That(result.Infobox!.Single().GetProperty("key").GetString(), Is.EqualTo("简体中文名"));
            Assert.That(result.BloodType, Is.EqualTo(BloodType.AB));
            Assert.That(result.BirthMonth, Is.EqualTo(1));
            Assert.That(result.Statistics.CommentCount, Is.EqualTo(3));
        });
    }

    [Test]
    public void PersonSearchRequest_SerializesOnlyRequestBodyFields()
    {
        var request = new PersonSearchRequest
        {
            Keyword = "声优",
            Filter  = new PersonSearchFilter { Careers = [PersonCareer.Seiyu, PersonCareer.Actor] },
            Limit   = 10,
            Offset  = 20
        };

        using var json    = JsonDocument.Parse(JsonSerializer.Serialize(request, SerializerOptions));
        var       root    = json.RootElement;
        var       careers = root.GetProperty("filter").GetProperty("career");

        Assert.Multiple(() =>
        {
            Assert.That(root.GetProperty("keyword").GetString(), Is.EqualTo("声优"));
            Assert.That(careers.EnumerateArray().Select(value => value.GetString()),
                        Is.EqualTo(new[] { "seiyu", "actor" }));
            Assert.That(root.TryGetProperty("limit", out _), Is.False);
            Assert.That(root.TryGetProperty("offset", out _), Is.False);
        });
    }

    [Test]
    public void PersonRelatedCharacter_DeserializesRequiredRelationFields()
    {
        const string json = """
            {"id":8,"name":"Hero","type":1,"images":null,"subject_id":42,"subject_type":2,"subject_name":"Subject","subject_name_cn":"条目","staff":"CV"}
            """;

        var result = JsonSerializer.Deserialize<PersonRelatedCharacter>(json, SerializerOptions);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result!.Type, Is.EqualTo(CharacterType.Character));
            Assert.That(result.SubjectId, Is.EqualTo(42));
            Assert.That(result.SubjectType, Is.EqualTo(SubjectType.Anime));
            Assert.That(result.SubjectNameCn, Is.EqualTo("条目"));
            Assert.That(result.Staff, Is.EqualTo("CV"));
        });
    }

    private static JsonSerializerOptions CreateSerializerOptions()
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new JsonStringEnumConverter<PersonCareer>(JsonNamingPolicy.CamelCase));
        return options;
    }
}
