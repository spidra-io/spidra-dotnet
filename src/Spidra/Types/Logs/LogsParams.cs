using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spidra.Types.Logs;

public class LogsParams
{
    public int? Limit { get; set; }
    public int? Offset { get; set; }
    public int? Page { get; set; }
    public string? Status { get; set; }
    public string? SearchTerm { get; set; }
    public string? DateStart { get; set; }
    public string? DateEnd { get; set; }

    public Dictionary<string, string?> ToQueryDictionary()
    {
        var dict = new Dictionary<string, string?>();
        if (Limit.HasValue) dict["limit"] = Limit.Value.ToString();
        if (Offset.HasValue) dict["offset"] = Offset.Value.ToString();
        if (Page.HasValue) dict["page"] = Page.Value.ToString();
        if (!string.IsNullOrWhiteSpace(Status)) dict["status"] = Status;
        if (!string.IsNullOrWhiteSpace(SearchTerm)) dict["searchTerm"] = SearchTerm;
        if (!string.IsNullOrWhiteSpace(DateStart)) dict["dateStart"] = DateStart;
        if (!string.IsNullOrWhiteSpace(DateEnd)) dict["dateEnd"] = DateEnd;
        return dict;
    }
}

public class ScrapeLogDetail
{
    [JsonPropertyName("uuid")]
    public required string Uuid { get; set; }

    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("createdAt")]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("data")]
    public JsonElement? Data { get; set; }
}

public class ScrapeLogsData
{
    [JsonPropertyName("logs")]
    public List<JsonElement> Logs { get; set; } = [];

    [JsonPropertyName("total")]
    public int Total { get; set; }
}

public class ScrapeLogsResponse
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("data")]
    public ScrapeLogsData? Data { get; set; }
}

public class SearchLog
{
    [JsonPropertyName("uuid")]
    public required string Uuid { get; set; }

    [JsonPropertyName("query")]
    public required string Query { get; set; }

    [JsonPropertyName("sources")]
    public List<string> Sources { get; set; } = [];

    [JsonPropertyName("createdAt")]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("credits")]
    public double Credits { get; set; }
}

public class SearchLogsData
{
    [JsonPropertyName("logs")]
    public List<SearchLog> Logs { get; set; } = [];

    [JsonPropertyName("total")]
    public int Total { get; set; }
}

public class SearchLogsResponse
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("data")]
    public SearchLogsData? Data { get; set; }
}

public class MapLog
{
    [JsonPropertyName("uuid")]
    public required string Uuid { get; set; }

    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("totalLinks")]
    public int TotalLinks { get; set; }

    [JsonPropertyName("createdAt")]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("credits")]
    public double Credits { get; set; }
}

public class MapLogsData
{
    [JsonPropertyName("logs")]
    public List<MapLog> Logs { get; set; } = [];

    [JsonPropertyName("total")]
    public int Total { get; set; }
}

public class MapLogsResponse
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("data")]
    public MapLogsData? Data { get; set; }
}

public class ParseLog
{
    [JsonPropertyName("uuid")]
    public required string Uuid { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("fileName")]
    public string? FileName { get; set; }

    [JsonPropertyName("mode")]
    public string? Mode { get; set; }

    [JsonPropertyName("pageCount")]
    public int PageCount { get; set; }

    [JsonPropertyName("createdAt")]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("credits")]
    public double Credits { get; set; }
}

public class ParseLogsData
{
    [JsonPropertyName("logs")]
    public List<ParseLog> Logs { get; set; } = [];

    [JsonPropertyName("total")]
    public int Total { get; set; }
}

public class ParseLogsResponse
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("data")]
    public ParseLogsData? Data { get; set; }
}
