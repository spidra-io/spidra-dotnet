using System.Text.Json.Serialization;

namespace Spidra.Types.Crawl;

public class CrawlParams
{
    [JsonPropertyName("baseUrl")]
    public required string BaseUrl { get; set; }

    [JsonPropertyName("crawlInstruction")]
    public required string CrawlInstruction { get; set; }

    [JsonPropertyName("transformInstruction")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TransformInstruction { get; set; }

    [JsonPropertyName("maxPages")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int MaxPages { get; set; }

    [JsonPropertyName("useProxy")]
    public bool UseProxy { get; set; }
}
