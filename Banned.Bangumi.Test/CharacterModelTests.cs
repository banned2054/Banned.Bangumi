using Banned.Bangumi.Models.Characters;
using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Subjects;
using System.Text.Json;

namespace Banned.Bangumi.Test;

[TestFixture]
public sealed class CharacterModelTests
{
    [Test]
    public void Character_DeserializesDeclaredFields()
    {
        const string json = """
            {
              "id": 1,
              "name": "Character",
              "type": 1,
              "images": {
                "large": "https://example.test/large.jpg",
                "medium": "https://example.test/medium.jpg",
                "small": "https://example.test/small.jpg",
                "grid": "https://example.test/grid.jpg"
              },
              "summary": "Summary",
              "locked": false,
              "infobox": [
                { "key": "简体中文名", "value": "角色" }
              ],
              "gender": "女",
              "blood_type": 2,
              "birth_year": 2000,
              "birth_mon": 3,
              "birth_day": 4,
              "stat": {
                "comments": 5,
                "collects": 6
              }
            }
            """;

        var character = JsonSerializer.Deserialize<Character>(json);

        Assert.Multiple(() =>
        {
            Assert.That(character!.Id, Is.EqualTo(1));
            Assert.That(character.Name, Is.EqualTo("Character"));
            Assert.That(character.Type, Is.EqualTo(CharacterType.Character));
            Assert.That(character.Images?.Grid, Is.EqualTo("https://example.test/grid.jpg"));
            Assert.That(character.Infobox?.Single().GetProperty("key").GetString(), Is.EqualTo("简体中文名"));
            Assert.That(character.BloodType, Is.EqualTo(BloodType.B));
            Assert.That(character.Statistics.CommentCount, Is.EqualTo(5));
            Assert.That(character.Statistics.CollectionCount, Is.EqualTo(6));
        });
    }

    [Test]
    public void CharacterSearchRequest_SerializesOnlyRequestBodyFields()
    {
        var request = new CharacterSearchRequest
        {
            Keyword = "keyword",
            Filter  = new CharacterSearchFilter { Nsfw = false },
            Limit   = 20,
            Offset  = 40,
        };

        var json = JsonSerializer.Serialize(request);

        Assert.That(json, Is.EqualTo("{\"keyword\":\"keyword\",\"filter\":{\"nsfw\":false}}"));
    }

    [Test]
    public void CharacterRelatedPerson_DeserializesRequiredRelationFields()
    {
        const string json = """
            {
              "id": 2,
              "name": "Person",
              "type": 1,
              "subject_id": 3,
              "subject_type": 2,
              "subject_name": "Subject",
              "subject_name_cn": "条目",
              "staff": "CV"
            }
            """;

        var person = JsonSerializer.Deserialize<CharacterRelatedPerson>(json);

        Assert.Multiple(() =>
        {
            Assert.That(person!.Id, Is.EqualTo(2));
            Assert.That(person.SubjectId, Is.EqualTo(3));
            Assert.That(person.SubjectType, Is.EqualTo(SubjectType.Anime));
            Assert.That(person.Staff, Is.EqualTo("CV"));
        });
    }
}
