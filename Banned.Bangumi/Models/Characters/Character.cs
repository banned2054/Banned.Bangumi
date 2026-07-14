using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Persons;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Characters;

/// <summary>
/// 表示 Bangumi 角色详情。<br/>
/// Represents Bangumi character details.
/// </summary>
public sealed record Character
{
    /// <summary>获取角色 ID。<br/>Gets the character ID.</summary>
    [JsonPropertyName("id")]
    public int Id { get; init; }

    /// <summary>获取角色名称。<br/>Gets the character name.</summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>获取角色资源类型。<br/>Gets the character resource type.</summary>
    [JsonPropertyName("type")]
    public CharacterType Type { get; init; }

    /// <summary>获取角色图片；无图片时为 <see langword="null"/>。<br/>Gets character images, or <see langword="null"/> when unavailable.</summary>
    [JsonPropertyName("images")]
    public PersonImages? Images { get; init; }

    /// <summary>获取角色简介。<br/>Gets the character summary.</summary>
    [JsonPropertyName("summary")]
    public string Summary { get; init; } = string.Empty;

    /// <summary>获取角色是否被锁定。<br/>Gets whether the character is locked.</summary>
    [JsonPropertyName("locked")]
    public bool Locked { get; init; }

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

    /// <summary>获取角色的评论和收藏统计。<br/>Gets comment and collection statistics for the character.</summary>
    [JsonPropertyName("stat")]
    public ResourceStatistics Statistics { get; init; } = new();
}
