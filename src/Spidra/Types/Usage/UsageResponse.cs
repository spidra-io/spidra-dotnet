using System.Text.Json.Serialization;

namespace Spidra.Types.Usage;

public class UsageResponse
{
    [JsonPropertyName("data")]
    public List<UsageRow> Data { get; set; } = [];
}

public class UsageRow
{
    [JsonPropertyName("date")]
    public required string Date { get; set; }

    [JsonPropertyName("requests")]
    public int Requests { get; set; }

    [JsonPropertyName("credits")]
    public double Credits { get; set; }

    [JsonPropertyName("tokens")]
    public int Tokens { get; set; }
}
