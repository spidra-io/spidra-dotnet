using System.Text.Json;
using System.Text.Json.Serialization;
using Spidra.Types.Scrape;

namespace Spidra.Types.Batch;

public class BatchScrapeParams
{
    [JsonPropertyName("urls")]
    public required List<string> Urls { get; set; }

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
