using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Characters;

/// <summary>
/// 表示角色搜索筛选条件。<br/>
/// Represents character search filters.
/// </summary>
public sealed record CharacterSearchFilter
{
    /// <summary>
    /// 获取或初始化 NSFW 筛选值；<see langword="true"/> 仅返回 NSFW 角色，<see langword="false"/> 仅返回非 NSFW 角色，<see langword="null"/> 不限制。<br/>
    /// Gets or initializes the NSFW filter; <see langword="true"/> returns only NSFW characters, <see langword="false"/> returns only non-NSFW characters, and <see langword="null"/> applies no restriction.
    /// </summary>
    [JsonPropertyName("nsfw")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Nsfw { get; init; }
}
