using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record Filter2
{
    [JsonPropertyName("nsfw")]
    public bool? Nsfw { get; init; }
}