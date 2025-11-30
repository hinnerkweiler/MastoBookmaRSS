using System.Text.Json.Serialization;

namespace MastoBookmaRss.Models;

public class MastodonAccount
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = "";

    [JsonPropertyName("acct")]
    public string Acct { get; set; } = "";

    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; } = "";
}
