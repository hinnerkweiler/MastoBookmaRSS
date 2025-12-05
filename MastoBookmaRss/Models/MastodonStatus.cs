using System.Text.Json.Serialization;

namespace MastoBookmaRss.Models;

public class MastodonStatus
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = default!;

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; } = "";

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("account")]
    public MastodonAccount? Account { get; set; }

    [JsonPropertyName("card")]
    public MastodonCard? Card { get; set; }
    
    [JsonPropertyName("media_attachments")]
    public IReadOnlyList<MastodonMediaAttachment>? MediaAttachments { get; init; }

}