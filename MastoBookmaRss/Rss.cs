using System.Text;
using System.Text.Encodings.Web;
using System.Xml.Linq;
using MastoBookmaRss.Models;

namespace MastoBookmaRss;

public static class Rss
{
    public static XDocument Build(IEnumerable<MastodonStatus?> statuses, Uri instanceUri)
    {
        var contentNs = XNamespace.Get("http://purl.org/rss/1.0/modules/content/");

        var now = DateTimeOffset.UtcNow;

        var channel = new XElement("channel",
            new XElement("title", $"Mastodon bookmarks for {instanceUri.Host}"),
            new XElement("link", instanceUri.ToString().TrimEnd('/')),
            new XElement("description", "Latest Mastodon bookmarks"),
            new XElement("lastBuildDate", now.ToString("r"))
        );

        foreach (var status in statuses)
        {
            if (status is null) continue;

            var mastodonUrl = status.Url ?? $"{instanceUri.Scheme}://{instanceUri.Host}/@{status.Account?.Acct ?? status.Account?.Username}/{status.Id}";
            var created = status.CreatedAt == default ? now : new DateTimeOffset(status.CreatedAt).ToUniversalTime();

            // Default title = first 80 chars of plain text
            var fallbackTitle = Truncate(RemoveHtmlTags(status.Content ?? string.Empty), 80);

            string link = mastodonUrl;
            string title = fallbackTitle;

            if (status.Card != null && !string.IsNullOrWhiteSpace(status.Card.Url))
            {
                link = status.Card.Url;
                title = !string.IsNullOrWhiteSpace(status.Card.Title)
                    ? status.Card.Title
                    : fallbackTitle;
            }

            var item = new XElement("item",
                new XElement("title", title),
                new XElement("link", link),
                new XElement("guid", mastodonUrl),
                new XElement("pubDate", created.ToString("r")),
                new XElement("author", status.Account?.DisplayName ?? status.Account?.Acct ?? status.Account?.Username ?? "")
            );

            // Original Mastodon content as content:encoded (HTML)
            var contentHtml = new StringBuilder();
            contentHtml.Append("<p><strong>Mastodon status:</strong> ");
            contentHtml.Append(HtmlEncoder.Default.Encode(mastodonUrl));
            contentHtml.Append("</p>");
            contentHtml.Append(status.Content ?? string.Empty);

            item.Add(new XElement(contentNs + "encoded", new XCData(contentHtml.ToString())));

            channel.Add(item);
        }

        var rss = new XDocument(
            new XDeclaration("1.0", "utf-8", "yes"),
            new XElement("rss",
                new XAttribute("version", "2.0"),
                new XAttribute(XNamespace.Xmlns + "content", contentNs),
                channel
            )
        );

        return rss;
    }

    static string Truncate(string text, int maxScalars)
    {
        if (string.IsNullOrEmpty(text)) return text;
        if (maxScalars <= 0) return string.Empty;

        var sb = new StringBuilder();
        int count = 0;
        bool truncated = false;

        foreach (var rune in text.EnumerateRunes())
        {
            if (count >= maxScalars)
            {
                truncated = true;
                break;
            }

            sb.Append(rune.ToString());
            count++;
        }

        if (truncated)
        {
            sb.Append('…');
        }

        return sb.ToString();
    }


    static string RemoveHtmlTags(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        // naive, good enough for titles
        var inside = false;
        var sb = new StringBuilder(input.Length);

        foreach (var c in input)
        {
            if (c == '<') { inside = true; continue; }
            if (c == '>') { inside = false; continue; }
            if (!inside) sb.Append(c);
        }

        return sb.ToString().Trim();
    }
}