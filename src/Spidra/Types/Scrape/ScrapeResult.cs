using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spidra.Types.Scrape;

public class ScrapeResult
{
    [JsonPropertyName("content")]
    public JsonElement Content { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }
}
