using System.Text.Json.Serialization;

namespace Spidra.Types.Usage;

public class UsageResponse
{
    [JsonPropertyName("creditsUsed")]
    public int CreditsUsed { get; set; }

    [JsonPropertyName("creditsRemaining")]
    public int CreditsRemaining { get; set; }

    [JsonPropertyName("creditsTotal")]
    public int CreditsTotal { get; set; }

    [JsonPropertyName("plan")]
    public string? Plan { get; set; }
}
