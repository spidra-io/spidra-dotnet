using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spidra.Types.Scrape;

public class ScrapeResult
{
    [JsonPropertyName("content")]
    public JsonElement Content { get; set; }

    [JsonPropertyName("screenshots")]
    public List<string> Screenshots { get; set; } = [];

    [JsonPropertyName("actions")]
    public List<ActionResult> Actions { get; set; } = [];

    [JsonPropertyName("error")]
    public string? Error { get; set; }
}
