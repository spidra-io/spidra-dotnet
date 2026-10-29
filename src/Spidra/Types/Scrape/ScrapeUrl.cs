using System.Text.Json.Serialization;

namespace Spidra.Types.Scrape;

/// <summary>A URL to scrape, with optional per-request configuration and browser actions.</summary>
public class ScrapeUrl
{
    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("actions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<BrowserAction>? Actions { get; set; }

    public ScrapeUrl(string url)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url);
        Url = url;
    }

    public ScrapeUrl(string url, List<BrowserAction> actions) : this(url)
    {
        Actions = actions;
    }
}
