using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spidra.Types.Scrape;

public class ScrapeParams
{
    [JsonPropertyName("urls")]
    public required List<ScrapeUrl> Urls { get; set; }

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

    [JsonPropertyName("actions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<BrowserAction>? Actions { get; set; }

    [JsonPropertyName("schema")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Schema { get; set; }

    public static ScrapeParams ForUrl(string url, string? prompt = null, OutputFormat? output = null) => new()
    {
        Urls = [new ScrapeUrl(url)],
        Prompt = prompt,
        Output = output
    };
}
