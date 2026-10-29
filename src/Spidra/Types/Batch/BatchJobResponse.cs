using System.Text.Json.Serialization;

namespace Spidra.Types.Batch;

public class BatchJobResponse
{
    [JsonPropertyName("batchId")]
    public required string BatchId { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("totalUrls")]
    public int TotalUrls { get; set; }

    [JsonPropertyName("completedCount")]
    public int CompletedCount { get; set; }

    [JsonPropertyName("failedCount")]
    public int FailedCount { get; set; }

    [JsonPropertyName("items")]
    public List<BatchItem> Items { get; set; } = [];
}

public class BatchCancelResult
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("refundedCredits")]
    public double RefundedCredits { get; set; }
}

public class BatchSummary
{
    [JsonPropertyName("uuid")]
    public required string Uuid { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("totalUrls")]
    public int TotalUrls { get; set; }

    [JsonPropertyName("completedCount")]
    public int CompletedCount { get; set; }

    [JsonPropertyName("failedCount")]
    public int FailedCount { get; set; }
}

public class BatchListResponse
{
    [JsonPropertyName("jobs")]
    public List<BatchSummary> Jobs { get; set; } = [];
}
