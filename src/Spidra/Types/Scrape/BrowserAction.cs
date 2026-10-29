using System.Text.Json.Serialization;

namespace Spidra.Types.Scrape;

/// <summary>
/// Specifies an interactive browser action (click, type, scroll, wait, forEach).
/// </summary>
public class BrowserAction
{
    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("selector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Selector { get; set; }

    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text { get; set; }

    [JsonPropertyName("milliseconds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Milliseconds { get; set; }

    [JsonPropertyName("actions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<BrowserAction>? Actions { get; set; }

    [JsonPropertyName("parameters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object?>? Parameters { get; set; }
}

