using System.Text.Json;
using System.Text.Json.Serialization;
using Spidra.Types.Scrape;

namespace Spidra.Types.Batch;

public class BatchScrapeParams
{
    [JsonPropertyName("urls")]
    public required List<string> Urls { get; set; }

    [JsonPropertyName("prompt")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Prompt { get; set; }

    [JsonPropertyName("output")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public OutputFormat? Output { get; set; }

    [JsonPropertyName("useProxy")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool UseProxy { get; set; }

    [JsonPropertyName("proxyCountry")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ProxyCountry { get; set; }

    [JsonPropertyName("screenshot")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Screenshot { get; set; }

    [JsonPropertyName("fullPageScreenshot")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool FullPageScreenshot { get; set; }

    [JsonPropertyName("extractContentOnly")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool ExtractContentOnly { get; set; }

    [JsonPropertyName("cookies")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Cookies { get; set; }

    [JsonPropertyName("schema")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Schema { get; set; }

    public static BatchScrapeParams ForUrls(List<string> urls, string? prompt = null, OutputFormat? output = null) => new()
    {
        Urls = urls,
        Prompt = prompt,
        Output = output
    };
}
