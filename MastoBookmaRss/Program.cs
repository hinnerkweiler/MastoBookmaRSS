using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using MastoBookmaRss;
using MastoBookmaRss.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient(); // generic HttpClientFactory

var app = builder.Build();

app.MapGet("/", () => Results.Redirect(Environment.GetEnvironmentVariable("REDIRECT_URL") ?? "https://aufmboot.com"));

app.MapGet("/conf", () =>
{
    var filePath = Path.Combine("wwwroot", "index.html");
    var html = File.ReadAllText(filePath);
    return Results.Content(html, "text/html; charset=utf-8");
});

app.MapGet("/feed", async (
    string? instance,
    string? token,
    int? limit,
    IHttpClientFactory httpClientFactory) =>
    {
        if (string.IsNullOrWhiteSpace(instance))
        {
            instance = Environment.GetEnvironmentVariable("MASTODON_INSTANCE_URL") ?? "mastodon.social";
        }
        
        if (string.IsNullOrWhiteSpace(token))
        {
            token = Environment.GetEnvironmentVariable("MASTODON_ACCESS_TOKEN") ?? string.Empty;
            if (string.IsNullOrEmpty(token)) return Results.BadRequest("Missing 'token' parameter or Environment Variable");
        }

        int maxItems;
        
        try
        {
            maxItems = 
                Math.Clamp(limit ?? int.Parse(Environment.GetEnvironmentVariable("BOOKMARKS_LIMIT") ?? "20"), 1, 100);
        }
        catch
        {
            maxItems = 20;
        }

        // Normalize instance to URI
        Uri? baseUri;
        if (!instance.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        {
            instance = "https://" + instance.Trim();
        }

        if (!Uri.TryCreate(instance, UriKind.Absolute, out baseUri))
        {
            return Results.BadRequest("Invalid 'instance' URL.");
        }

        // Optional safety: only allow HTTPS and non-local hosts
        if (baseUri.Scheme != Uri.UriSchemeHttps && baseUri.Scheme != Uri.UriSchemeHttp)
        {
            return Results.BadRequest("Only http/https instances are supported.");
        }

        var client = httpClientFactory.CreateClient();
        client.BaseAddress = new Uri($"{baseUri.Scheme}://{baseUri.Host}");
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var requestUri = $"/api/v1/bookmarks?limit={maxItems}";
        var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();

            return Results.Problem(detail: body, statusCode: (int)response.StatusCode,
                title: $"Error from Mastodon instance: {response.ReasonPhrase}");

        }

        await using var stream = await response.Content.ReadAsStreamAsync();
        var statuses = await JsonSerializer.DeserializeAsync<List<MastodonStatus>>(stream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        statuses ??= new List<MastodonStatus>();

        // Build RSS 2.0
        var rss = Rss.Build(statuses, baseUri);

        var xml = rss.Declaration + Environment.NewLine + rss.ToString(SaveOptions.DisableFormatting);
        return Results.Content(xml, "application/rss+xml; charset=utf-8");
    });

app.Run();

