using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record LegacySubjectLarge : LegacySubjectMedium
{
    /// <summary>
    /// 章节列表
    /// </summary>
    [JsonPropertyName("eps")]
    public new ICollection<LegacyEpisode>? Eps { get; init; }
    [JsonPropertyName("topic")]
    public ICollection<LegacyTopic>? Topic { get; init; }
    /// <summary>
    /// 评论日志
    /// </summary>
    [JsonPropertyName("blog")]
    public ICollection<LegacyBlog>? Blog { get; init; }
}
