using Banned.Bangumi.Models.Common;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Persons;

/// <summary>
/// 表示 Bangumi 人物详情。<br/>
/// Represents Bangumi person details.
/// </summary>
public sealed record PersonDetail
{
    /// <summary>获取人物 ID。<br/>Gets the person ID.</summary>
    [JsonPropertyName("id")]
    public int Id { get; init; }

    /// <summary>获取人物名称。<br/>Gets the person name.</summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>获取人物类型。<br/>Gets the person type.</summary>
    [JsonPropertyName("type")]
    public PersonType Type { get; init; }

    /// <summary>获取人物职业。<br/>Gets the person's careers.</summary>
    [JsonPropertyName("career")]
    public IReadOnlyList<PersonCareer> Careers { get; init; } = [];

    /// <summary>获取人物图片；无图片时为 <see langword="null"/>。<br/>Gets person images, or <see langword="null"/> when unavailable.</summary>
    [JsonPropertyName("images")]
    public Images? Images { get; init; }

    /// <summary>获取人物简介。<br/>Gets the person summary.</summary>
    [JsonPropertyName("summary")]
    public string Summary { get; init; } = string.Empty;

    /// <summary>获取人物是否被锁定。<br/>Gets whether the person is locked.</summary>
    [JsonPropertyName("locked")]
    public bool Locked { get; init; }

    /// <summary>获取服务端记录的最后修改时间。<br/>Gets the last modification time recorded by the server.</summary>
    [JsonPropertyName("last_modified")]
    public DateTimeOffset LastModified { get; init; }

    /// <summary>获取服务端从 Wiki 解析的信息框；解析无效时为 <see langword="null"/>。<br/>Gets the infobox parsed from the wiki by the server, or <see langword="null"/> when parsing is invalid.</summary>
    [JsonPropertyName("infobox")]
    public IReadOnlyList<JsonElement>? Infobox { get; init; }

    /// <summary>获取服务端从 Wiki 解析的性别。<br/>Gets the gender parsed from the wiki by the server.</summary>
    [JsonPropertyName("gender")]
    public string? Gender { get; init; }

    /// <summary>获取服务端从 Wiki 解析的血型。<br/>Gets the blood type parsed from the wiki by the server.</summary>
    [JsonPropertyName("blood_type")]
    public BloodType? BloodType { get; init; }

    /// <summary>获取服务端从 Wiki 解析的出生年份。<br/>Gets the birth year parsed from the wiki by the server.</summary>
    [JsonPropertyName("birth_year")]
    public int? BirthYear { get; init; }

    /// <summary>获取服务端从 Wiki 解析的出生月份。<br/>Gets the birth month parsed from the wiki by the server.</summary>
    [JsonPropertyName("birth_mon")]
    public int? BirthMonth { get; init; }

    /// <summary>获取服务端从 Wiki 解析的出生日期。<br/>Gets the day of birth parsed from the wiki by the server.</summary>
    [JsonPropertyName("birth_day")]
    public int? BirthDay { get; init; }

    /// <summary>获取人物的评论和收藏统计。<br/>Gets comment and collection statistics for the person.</summary>
    [JsonPropertyName("stat")]
    public ResourceStatistics Statistics { get; init; } = new();
}
