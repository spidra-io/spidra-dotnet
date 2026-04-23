using System.Text.Json.Serialization;

namespace Spidra.Types.Scrape;

/// <summary>A URL to scrape, with optional per-request configuration.</summary>
public class ScrapeUrl
{
    [JsonPropertyName("url")]
    public string Url { get; }

    public ScrapeUrl(string url)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url);
        Url = url;
    }
}
