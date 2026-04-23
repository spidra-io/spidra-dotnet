using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spidra.Types.Crawl;

public class CrawlPage
{
    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("data")]
    public JsonElement Data { get; set; }
}
