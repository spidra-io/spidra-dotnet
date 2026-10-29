using System.Text.Json.Serialization;

namespace Spidra.Types.Scrape;

public class ScrapeJobResponse
{
    [JsonPropertyName("jobId")]
    public required string JobId { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("result")]
    public ScrapeResult? Result { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }
}
