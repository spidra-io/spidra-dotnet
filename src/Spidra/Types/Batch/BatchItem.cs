using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spidra.Types.Batch;

public class BatchItem
{
    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("result")]
    public JsonElement? Result { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }
}
