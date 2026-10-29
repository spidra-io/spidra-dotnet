using System.Text.Json.Serialization;

namespace Spidra.Types.Map;

public class MapParams
{
    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("search")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Search { get; set; }

    [JsonPropertyName("ignoreSitemap")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IgnoreSitemap { get; set; }

    [JsonPropertyName("includeSubdomains")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IncludeSubdomains { get; set; }

    [JsonPropertyName("limit")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Limit { get; set; }

    [JsonPropertyName("useProxy")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool UseProxy { get; set; }

    [JsonPropertyName("proxyCountry")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ProxyCountry { get; set; }

    public static MapParams ForUrl(string url, string? search = null) => new()
    {
        Url = url,
        Search = search
    };
}

public class MapLink
{
    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}

public class MapResult
{
    [JsonPropertyName("links")]
    public List<MapLink> Links { get; set; } = [];

    [JsonPropertyName("totalLinks")]
    public int TotalLinks { get; set; }

    [JsonPropertyName("executionTimeMs")]
    public long ExecutionTimeMs { get; set; }
}

