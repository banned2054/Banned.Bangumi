using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

/// <summary>
/// 新增或修改条目的内容，同名字段意义同&lt;a href="#model-Subject"&gt;Subject&lt;/a&gt;
/// </summary>
public partial record IndexBasicInfo
{
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }
}