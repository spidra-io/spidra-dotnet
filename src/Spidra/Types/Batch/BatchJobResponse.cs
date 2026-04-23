using System.Text.Json.Serialization;

namespace Spidra.Types.Batch;

public class BatchJobResponse
{
    [JsonPropertyName("batchId")]
    public required string BatchId { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("items")]
    public List<BatchItem> Items { get; set; } = [];
}
