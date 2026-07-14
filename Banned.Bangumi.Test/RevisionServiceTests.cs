using Banned.Bangumi.Exceptions;
using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Revisions;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Banned.Bangumi.Test;

[TestFixture]
public sealed class RevisionServiceTests
{
    [TestCase("persons", "person_id")]
    [TestCase("characters", "character_id")]
    [TestCase("subjects", "subject_id")]
    [TestCase("episodes", "episode_id")]
    public async Task ListMethods_SendExpectedQueryWithoutAuthenticationAndDeserializePage(string resourcePath,
                                                                                           string resourceIdName)
    {
        var handler = CreateJsonHandler("""
                                        {
                                          "total": 3,
                                          "limit": 20,
                                          "offset": 10,
                                          "data": [
                                            {
                                              "id": 7,
                                              "type": 2,
                                              "creator": { "username": "sai", "nickname": "Sai" },
                                              "summary": "修订摘要",
                                              "created_at": "2026-07-14T09:30:00+08:00"
                                            }
                                          ]
                                        }
                                        """);
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "must-not-be-sent");

        var result = resourcePath switch
        {
            "persons"    => await client.Revisions.GetPersons(42, 20, 10),
            "characters" => await client.Revisions.GetCharacters(42, 20, 10),
            "subjects"   => await client.Revisions.GetSubjects(42, 20, 10),
            "episodes"   => await client.Revisions.GetEpisodes(42, 20, 10),
            _            => throw new ArgumentOutOfRangeException(nameof(resourcePath), resourcePath, null)
        };

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        var revision = result.Data.Single();
        Assert.Multiple(() =>
        {
            Assert.That(request!.Method, Is.EqualTo(HttpMethod.Get));
            Assert.That(request.Uri, Is.EqualTo(new Uri(
                            $"https://api.example/root/v0/revisions/{resourcePath}?{resourceIdName}=42&limit=20&offset=10")));
            Assert.That(request.Authorization, Is.Null);
            Assert.That(request.Body, Is.Null);
            Assert.That(result.Total, Is.EqualTo(3));
            Assert.That(result.Limit, Is.EqualTo(20));
            Assert.That(result.Offset, Is.EqualTo(10));
            Assert.That(revision.Id, Is.EqualTo(7));
            Assert.That(revision.Type, Is.EqualTo(2));
            Assert.That(revision.Creator!.Username, Is.EqualTo("sai"));
            Assert.That(revision.Creator.Nickname, Is.EqualTo("Sai"));
            Assert.That(revision.Summary, Is.EqualTo("修订摘要"));
            Assert.That(revision.CreatedAt.Offset, Is.EqualTo(TimeSpan.FromHours(8)));
        });
    }

    [Test]
    public async Task GetPerson_SendsExpectedPathAndDeserializesDetailedData()
    {
        var handler = CreateJsonHandler("""
                                        {
                                          "id": 101,
                                          "type": 1,
                                          "summary": "人物修订",
                                          "created_at": "2026-07-14T01:00:00Z",
                                          "data": {
                                            "42": {
                                              "prsn_infobox": "简体中文名=测试人物",
                                              "prsn_summary": "人物简介",
                                              "profession": { "seiyu": "1", "actor": "0" },
                                              "extra": { "img": "person/42.jpg" },
                                              "prsn_name": "Test Person"
                                            }
                                          }
                                        }
                                        """);
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient, accessToken : "must-not-be-sent");

        var result = await client.Revisions.GetPerson(101);

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        var data = result.Data["42"];
        Assert.Multiple(() =>
        {
            Assert.That(request!.Method, Is.EqualTo(HttpMethod.Get));
            Assert.That(request.Uri,
                        Is.EqualTo(new Uri("https://api.example/root/v0/revisions/persons/101")));
            Assert.That(request.Authorization, Is.Null);
            Assert.That(result.Id, Is.EqualTo(101));
            Assert.That(data.Infobox, Is.EqualTo("简体中文名=测试人物"));
            Assert.That(data.Summary, Is.EqualTo("人物简介"));
            Assert.That(data.Profession.VoiceActor, Is.EqualTo("1"));
            Assert.That(data.Profession.Actor, Is.EqualTo("0"));
            Assert.That(data.Extra.Image, Is.EqualTo("person/42.jpg"));
            Assert.That(data.Name, Is.EqualTo("Test Person"));
        });
    }

    [Test]
    public async Task GetCharacter_SendsExpectedPathAndDeserializesDetailedData()
    {
        var handler = CreateJsonHandler("""
                                        {
                                          "id": 102,
                                          "type": 1,
                                          "summary": "角色修订",
                                          "created_at": "2026-07-14T01:00:00Z",
                                          "data": {
                                            "9": {
                                              "infobox": "性别=女",
                                              "summary": "角色简介",
                                              "name": "Test Character",
                                              "extra": { "img": "character/9.jpg" }
                                            }
                                          }
                                        }
                                        """);
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient);

        var result = await client.Revisions.GetCharacter(102);

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        var data = result.Data["9"];
        Assert.Multiple(() =>
        {
            Assert.That(request!.Uri,
                        Is.EqualTo(new Uri("https://api.example/root/v0/revisions/characters/102")));
            Assert.That(data.Infobox, Is.EqualTo("性别=女"));
            Assert.That(data.Summary, Is.EqualTo("角色简介"));
            Assert.That(data.Name, Is.EqualTo("Test Character"));
            Assert.That(data.Extra.Image, Is.EqualTo("character/9.jpg"));
        });
    }

    [Test]
    public async Task GetSubject_SendsExpectedPathAndDeserializesDetailedData()
    {
        var handler = CreateJsonHandler("""
                                        {
                                          "id": 103,
                                          "type": 1,
                                          "summary": "条目修订",
                                          "created_at": "2026-07-14T01:00:00Z",
                                          "data": {
                                            "field_eps": 12,
                                            "field_infobox": "导演=测试",
                                            "field_summary": "条目简介",
                                            "name": "Test Subject",
                                            "name_cn": "测试条目",
                                            "platform": 1,
                                            "subject_id": 42,
                                            "type": 2,
                                            "type_id": 2,
                                            "vote_field": "vote"
                                          }
                                        }
                                        """);
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient);

        var result = await client.Revisions.GetSubject(103);

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(request!.Uri,
                        Is.EqualTo(new Uri("https://api.example/root/v0/revisions/subjects/103")));
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data!.EpisodeCount, Is.EqualTo(12));
            Assert.That(result.Data.Infobox, Is.EqualTo("导演=测试"));
            Assert.That(result.Data.NameCn, Is.EqualTo("测试条目"));
            Assert.That(result.Data.SubjectId, Is.EqualTo(42));
            Assert.That(result.Data.VoteField, Is.EqualTo("vote"));
        });
    }

    [Test]
    public async Task GetEpisode_PreservesDynamicDetailedDataAsJson()
    {
        var handler = CreateJsonHandler("""
                                        {
                                          "id": 104,
                                          "type": 1,
                                          "summary": "章节修订",
                                          "created_at": "2026-07-14T01:00:00Z",
                                          "data": {
                                            "name": "Episode 1",
                                            "sort": 1.5,
                                            "nested": { "enabled": true }
                                          }
                                        }
                                        """);
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient);

        var result = await client.Revisions.GetEpisode(104);

        Assert.That(handler.Requests.TryDequeue(out var request), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(request!.Uri,
                        Is.EqualTo(new Uri("https://api.example/root/v0/revisions/episodes/104")));
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data!.Value.ValueKind, Is.EqualTo(JsonValueKind.Object));
            Assert.That(result.Data.Value.GetProperty("name").GetString(), Is.EqualTo("Episode 1"));
            Assert.That(result.Data.Value.GetProperty("sort").GetDouble(), Is.EqualTo(1.5));
            Assert.That(result.Data.Value.GetProperty("nested").GetProperty("enabled").GetBoolean(), Is.True);
        });
    }

    [Test]
    public void GetSubject_MapsNotFoundResponseToApiException()
    {
        var handler = CreateJsonHandler(
                                        "{\"title\":\"Not Found\",\"description\":\"Revision does not exist\"}",
                                        HttpStatusCode.NotFound);
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient);

        var exception = Assert.ThrowsAsync<BangumiApiException>(async () => await client.Revisions.GetSubject(999));

        Assert.Multiple(() =>
        {
            Assert.That(exception!.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.Message, Is.EqualTo("Revision does not exist"));
        });
    }

    [Test]
    public void GetEpisodes_PropagatesCancellationToken()
    {
        var handler = new TestHttpMessageHandler(async (_, cancellationToken) =>
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            throw new InvalidOperationException("The request should have been cancelled.");
        });
        using var httpClient         = new HttpClient(handler);
        using var client             = CreateClient(httpClient);
        using var cancellationSource = new CancellationTokenSource();
        cancellationSource.Cancel();

        Assert.That(
                    async () => await client.Revisions.GetEpisodes(42, cancellationToken : cancellationSource.Token),
                    Throws.InstanceOf<OperationCanceledException>());
    }

    [Test]
    public void PublicMethods_RejectInvalidArgumentsBeforeSendingRequests()
    {
        var       handler    = CreateJsonHandler("{}");
        using var httpClient = new HttpClient(handler);
        using var client     = CreateClient(httpClient);

        Assert.Multiple(() =>
        {
            Assert.That(async () => await client.Revisions.GetPersons(0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Revisions.GetCharacters(0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Revisions.GetSubjects(0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Revisions.GetEpisodes(0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Revisions.GetPersons(1, 0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Revisions.GetPersons(1, 51),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Revisions.GetPersons(1, offset : -1),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Revisions.GetPerson(0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Revisions.GetCharacter(0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Revisions.GetSubject(0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(async () => await client.Revisions.GetEpisode(0),
                        Throws.TypeOf<ArgumentOutOfRangeException>());
        });
        Assert.That(handler.Requests, Is.Empty);
    }

    private static TestHttpMessageHandler CreateJsonHandler(
        string         json,
        HttpStatusCode statusCode = HttpStatusCode.OK) =>
        new((_, _) => Task.FromResult(new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        }));

    private static BangumiClient CreateClient(HttpClient httpClient, string? accessToken = null) =>
        new(new BangumiClientOptions
        {
            BaseAddress = new Uri("https://api.example/root"),
            AccessToken = accessToken,
            UserAgent   = "Banned.Bangumi.Test/1.0",
            HttpClient  = httpClient
        });
}
