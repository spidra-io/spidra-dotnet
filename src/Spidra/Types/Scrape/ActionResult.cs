using System.Text.Json.Serialization;

namespace Spidra.Types.Scrape;

/// <summary>
/// Execution metrics and status for an individual browser action.
/// </summary>
public class ActionResult
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("durationMs")]
    public long DurationMs { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }
}

