using System.Text.Json.Serialization;

namespace Spidra.Types.Logs;

public class ScrapeLog
{
    [JsonPropertyName("jobId")]
    public required string JobId { get; set; }

    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("completedAt")]
    public DateTimeOffset? CompletedAt { get; set; }

    [JsonPropertyName("prompt")]
    public string? Prompt { get; set; }
}
