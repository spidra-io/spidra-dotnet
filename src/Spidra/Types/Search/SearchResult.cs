using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spidra.Types.Search;

public class SearchResultItem
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("snippet")]
    public string? Snippet { get; set; }

    [JsonPropertyName("date")]
    public string? Date { get; set; }

    [JsonPropertyName("source")]
    public string? Source { get; set; }

    [JsonPropertyName("image")]
    public string? Image { get; set; }

    [JsonPropertyName("thumbnail")]
    public string? Thumbnail { get; set; }

    [JsonPropertyName("scrapedContent")]
    public JsonElement? ScrapedContent { get; set; }
}

public class SearchResponseData
{
    [JsonPropertyName("web")]
    public List<SearchResultItem> Web { get; set; } = [];

    [JsonPropertyName("news")]
    public List<SearchResultItem> News { get; set; } = [];

    [JsonPropertyName("images")]
    public List<SearchResultItem> Images { get; set; } = [];

    [JsonPropertyName("videos")]
    public List<SearchResultItem> Videos { get; set; } = [];

    [JsonPropertyName("research")]
    public List<SearchResultItem> Research { get; set; } = [];

    [JsonPropertyName("developer")]
    public List<SearchResultItem> Developer { get; set; } = [];
}

public class SearchExtras
{
    [JsonPropertyName("answer")]
    public string? Answer { get; set; }

    [JsonPropertyName("relatedQuestions")]
    public List<string> RelatedQuestions { get; set; } = [];
}

public class SearchStats
{
    [JsonPropertyName("totalResults")]
    public int TotalResults { get; set; }

    [JsonPropertyName("executionTimeMs")]
    public long ExecutionTimeMs { get; set; }
}

public class SearchResult
{
    [JsonPropertyName("query")]
    public string? Query { get; set; }

    [JsonPropertyName("data")]
    public SearchResponseData? Data { get; set; }

    [JsonPropertyName("results")]
    public List<SearchResultItem> Results { get; set; } = [];

    [JsonPropertyName("extras")]
    public SearchExtras? Extras { get; set; }

    [JsonPropertyName("stats")]
    public SearchStats? Stats { get; set; }
}

public class SearchJobResponse
{
    [JsonPropertyName("jobId")]
    public required string JobId { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("query")]
    public string? Query { get; set; }

    [JsonPropertyName("result")]
    public SearchResult? Result { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }
}

