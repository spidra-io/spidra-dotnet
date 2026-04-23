using System.Text.Json.Serialization;

namespace Spidra.Types.Scrape;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OutputFormat
{
    [JsonPropertyName("json")]
    Json,

    [JsonPropertyName("markdown")]
    Markdown,

    [JsonPropertyName("text")]
    Text
}
