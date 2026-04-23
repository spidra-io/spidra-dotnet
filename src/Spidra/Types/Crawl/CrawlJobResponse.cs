using System.Text.Json.Serialization;

namespace Spidra.Types.Crawl;

public class CrawlJobResponse
{
    [JsonPropertyName("jobId")]
    public required string JobId { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("result")]
    public List<CrawlPage> Result { get; set; } = [];
}
