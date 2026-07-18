using Banned.Bangumi.Models.Common;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Characters;

/// <summary>
/// 表示角色搜索筛选条件。<br/>
/// Represents character search filters.
/// </summary>
public sealed record CharacterSearchFilter
{
    /// <summary>
    /// 获取或初始化 NSFW 搜索限制；<see langword="null"/> 不限制结果。<br/>
    /// Gets or initializes the NSFW search restriction; <see langword="null"/> does not restrict results.
    /// </summary>
    /// <remarks>
    /// 无权限的用户设置此属性时，服务端会忽略该筛选条件且不会返回 NSFW 角色。<br/>
    /// The server ignores this filter for unauthorized users and does not return NSFW characters.
    /// </remarks>
    [JsonPropertyName("nsfw")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NsfwFilterMode? NsfwFilter { get; init; }
}
