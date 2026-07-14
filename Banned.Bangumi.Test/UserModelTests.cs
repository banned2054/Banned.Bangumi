using Banned.Bangumi.Models;
using Banned.Bangumi.Models.Users;
using System.Text.Json;

namespace Banned.Bangumi.Test;

[TestFixture]
public sealed class UserModelTests
{
    [Test]
    public void LegacyUser_DeserializesSharedUserGroup()
    {
        const string json = """
            {"id":1,"username":"sai","nickname":"Sai","usergroup":11}
            """;

        var user = JsonSerializer.Deserialize<LegacyUser>(json);

        Assert.That(user!.UserGroup, Is.EqualTo(UserGroup.WikiUser));
    }

    [Test]
    public void CurrentUser_DeserializesDeclaredFieldsAndIgnoresUndeclaredUrl()
    {
        const string json = """
            {
              "id": 1,
              "username": "sai",
              "nickname": "Sai",
              "user_group": 1,
              "avatar": {
                "large": "https://example.test/large.jpg",
                "medium": "https://example.test/medium.jpg",
                "small": "https://example.test/small.jpg"
              },
              "sign": "Awesome!",
              "email": "sai@example.test",
              "reg_time": "2017-12-03T08:51:16+08:00",
              "time_offset": 8,
              "url": "https://bgm.tv/user/sai"
            }
            """;

        var user = JsonSerializer.Deserialize<CurrentUser>(json);

        Assert.Multiple(() =>
        {
            Assert.That(user!.Id, Is.EqualTo(1));
            Assert.That(user.Username, Is.EqualTo("sai"));
            Assert.That(user.UserGroup, Is.EqualTo(UserGroup.Admin));
            Assert.That(user.Avatar.Medium, Is.EqualTo("https://example.test/medium.jpg"));
            Assert.That(user.Email, Is.EqualTo("sai@example.test"));
            Assert.That(user.RegistrationTime, Is.EqualTo(DateTimeOffset.Parse("2017-12-03T08:51:16+08:00")));
            Assert.That(user.TimeOffset, Is.EqualTo(8));
        });
    }
}
