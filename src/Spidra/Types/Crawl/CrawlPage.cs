using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spidra.Types.Crawl;

public class CrawlPage
{
    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("data")]
    public JsonElement Data { get; set; }

    [JsonPropertyName("html_url")]
    public string? HtmlUrl { get; set; }

    [JsonPropertyName("markdown_url")]
    public string? MarkdownUrl { get; set; }

    [JsonPropertyName("htmlKey")]
    public string? HtmlKey { get; set; }

    [JsonPropertyName("markdownKey")]
    public string? MarkdownKey { get; set; }
}
