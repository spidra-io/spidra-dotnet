using System.Text.Json.Serialization;

namespace Spidra.Types.Crawl;

public class CrawlParams
{
    [JsonPropertyName("baseUrl")]
    public required string BaseUrl { get; set; }

    [JsonPropertyName("crawlInstruction")]
    public required string CrawlInstruction { get; set; }

    [JsonPropertyName("transformInstruction")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TransformInstruction { get; set; }

    [JsonPropertyName("maxPages")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int MaxPages { get; set; }

    [JsonPropertyName("useProxy")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool UseProxy { get; set; }

    [JsonPropertyName("proxyCountry")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ProxyCountry { get; set; }

    [JsonPropertyName("cookies")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Cookies { get; set; }

    public static CrawlParams ForUrl(string baseUrl, string? crawlInstruction = null, string? transformInstruction = null) => new()
    {
        BaseUrl = baseUrl,
        CrawlInstruction = crawlInstruction ?? "Follow all internal links",
        TransformInstruction = transformInstruction
    };
}
