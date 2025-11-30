using System.Text.Json.Serialization;

namespace MastoBookmaRss.Models;

public class MastodonCard
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = "";

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}