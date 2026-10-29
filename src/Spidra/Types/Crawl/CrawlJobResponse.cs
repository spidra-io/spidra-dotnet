using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spidra.Types.Crawl;

public class CrawlJobResponse
{
    [JsonPropertyName("jobId")]
    public required string JobId { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("progress")]
    public JsonElement? Progress { get; set; }

    [JsonPropertyName("result")]
    public List<CrawlPage> Result { get; set; } = [];

    [JsonPropertyName("error")]
    public string? Error { get; set; }
}

public class CrawlCancelResult
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("refundedCredits")]
    public double RefundedCredits { get; set; }
}

public class CrawlReport
{
    [JsonPropertyName("skippedUrls")]
    public List<Dictionary<string, object?>> SkippedUrls { get; set; } = [];

    [JsonPropertyName("errors")]
    public List<Dictionary<string, object?>> Errors { get; set; } = [];
}

public class CrawlReportResponse
{
    [JsonPropertyName("jobId")]
    public required string JobId { get; set; }

    [JsonPropertyName("report")]
    public CrawlReport? Report { get; set; }
}

public class CrawlRetryPageResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("pageId")]
    public required string PageId { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }
}

public class CrawlPagesResponse
{
    [JsonPropertyName("pages")]
    public List<CrawlPage> Pages { get; set; } = [];
}

public class CrawlSummary
{
    [JsonPropertyName("uuid")]
    public required string Uuid { get; set; }

    [JsonPropertyName("base_url")]
    public required string BaseUrl { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("pages_crawled")]
    public int PagesCrawled { get; set; }
}

public class CrawlHistory
{
    [JsonPropertyName("jobs")]
    public List<CrawlSummary> Jobs { get; set; } = [];
}

public class CrawlStats
{
    [JsonPropertyName("total")]
    public int Total { get; set; }
}

public class CrawlExtractResponse
{
    [JsonPropertyName("jobId")]
    public required string JobId { get; set; }
}
