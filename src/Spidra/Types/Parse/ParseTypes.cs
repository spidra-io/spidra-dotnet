using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spidra.Types.Parse;

public class ParseParams
{
    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url { get; set; }

    [JsonPropertyName("fileId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FileId { get; set; }

    [JsonPropertyName("fileName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FileName { get; set; }

    [JsonPropertyName("pages")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Pages { get; set; }

    [JsonPropertyName("includePages")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<int>? IncludePages { get; set; }

    [JsonPropertyName("prompt")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Prompt { get; set; }

    [JsonPropertyName("mode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Mode { get; set; } // "ai", "fast"

    [JsonPropertyName("wait")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Wait { get; set; }

    public static ParseParams ForUrl(string url, string? mode = null) => new()
    {
        Url = url,
        Mode = mode
    };
}

public class ParsedContent
{
    [JsonPropertyName("markdown")]
    public string? Markdown { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("data")]
    public JsonElement? Data { get; set; }
}

public class ParsedPage
{
    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; }

    [JsonPropertyName("markdown")]
    public string? Markdown { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("images")]
    public List<string> Images { get; set; } = [];
}

public class ParseJobResponse
{
    [JsonPropertyName("jobId")]
    public required string JobId { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("result")]
    public ParsedContent? Result { get; set; }

    [JsonPropertyName("pages")]
    public List<ParsedPage> Pages { get; set; } = [];

    [JsonPropertyName("pageCount")]
    public int PageCount { get; set; }

    [JsonPropertyName("totalCredits")]
    public double TotalCredits { get; set; }

    [JsonPropertyName("executionTimeMs")]
    public long ExecutionTimeMs { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }
}

public class ParseEstimate
{
    [JsonPropertyName("estimatedCredits")]
    public double EstimatedCredits { get; set; }

    [JsonPropertyName("pageCount")]
    public int PageCount { get; set; }

    [JsonPropertyName("durationEstimateMs")]
    public long DurationEstimateMs { get; set; }
}

public class ParseBatchResponse
{
    [JsonPropertyName("batchId")]
    public required string BatchId { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("totalFiles")]
    public int TotalFiles { get; set; }

    [JsonPropertyName("completedCount")]
    public int CompletedCount { get; set; }

    [JsonPropertyName("failedCount")]
    public int FailedCount { get; set; }

    [JsonPropertyName("items")]
    public List<ParseJobResponse> Items { get; set; } = [];
}

