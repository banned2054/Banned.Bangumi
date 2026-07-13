using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record Response2 : Page
{
    [JsonPropertyName("data")]
    public ICollection<UserEpisodeCollection>? Data { get; init; }
}