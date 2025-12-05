namespace MastoBookmaRss.Models;

public sealed record MastodonMediaAttachment
{
    public string? Type { get; init; }
    public string? Url { get; init; }
    public string? PreviewUrl { get; init; }
    public string? MimeType { get; init; }
}