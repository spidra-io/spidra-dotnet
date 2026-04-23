using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spidra.Types.Scrape;

public class ScrapeParams
{
    [JsonPropertyName("urls")]
    public required List<ScrapeUrl> Urls { get; set; }

    [JsonPropertyName("prompt")]
    public required string Prompt { get; set; }

    [JsonPropertyName("output")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public OutputFormat? Output { get; set; }

    [JsonPropertyName("useProxy")]
    public bool UseProxy { get; set; }

    [JsonPropertyName("schema")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JsonElement? Schema { get; set; }
}
