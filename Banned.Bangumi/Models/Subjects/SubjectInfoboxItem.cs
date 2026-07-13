using System.Text.Json;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Subjects;

/// <summary>
/// 表示条目信息框中的一个字段。<br/>
/// Represents a field in a subject infobox.
/// </summary>
public sealed record SubjectInfoboxItem
{
    /// <summary>获取字段名称。<br/>Gets the field name.</summary>
    [JsonPropertyName("key")]
    public string Key { get; init; } = string.Empty;

    /// <summary>获取字符串或结构化数组形式的字段值。<br/>Gets the field value as either a string or a structured array.</summary>
    [JsonPropertyName("value")]
    public JsonElement Value { get; init; }
}
