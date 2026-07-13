using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record Subject
{
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    [JsonPropertyName("type")]
    public int? Type { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("name_cn")]
    public string? NameCn { get; init; }

    [JsonPropertyName("summary")]
    public string? Summary { get; init; }

    [JsonPropertyName("series")]
    public bool? Series { get; init; }

    [JsonPropertyName("nsfw")]
    public bool? Nsfw { get; init; }

    [JsonPropertyName("locked")]
    public bool? Locked { get; init; }

    /// <summary>
    /// air date in `YYYY-MM-DD` format
    /// </summary>
    [JsonPropertyName("date")]
    public string? Date { get; init; }

    [JsonPropertyName("platform")]
    public string? Platform { get; init; }

    [JsonPropertyName("images")]
    public Images? Images { get; init; }

    [JsonPropertyName("infobox")]
    public WikiV0? Infobox { get; init; }

    [JsonPropertyName("volumes")]
    public int? Volumes { get; init; }

    /// <summary>
    /// 由旧服务端从wiki中解析，对于书籍条目为`话数`
    /// </summary>
    [JsonPropertyName("eps")]
    public int? Eps { get; init; }

    [JsonPropertyName("total_episodes")]
    public int? TotalEpisodes { get; init; }

    [JsonPropertyName("rating")]
    public Rating2? Rating { get; init; }

    [JsonPropertyName("collection")]
    public Collection? Collection { get; init; }

    [JsonPropertyName("meta_tags")]
    public ICollection<string>? MetaTags { get; init; }

    [JsonPropertyName("tags")]
    public SubjectTags? Tags { get; init; }
}