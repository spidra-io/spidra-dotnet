using System.Text.Json.Serialization;

namespace Spidra.Types.Account;

public class AccountPlan
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("renewsAt")]
    public string? RenewsAt { get; set; }
}

public class AccountCredits
{
    [JsonPropertyName("available")]
    public double Available { get; set; }

    [JsonPropertyName("total")]
    public double Total { get; set; }

    [JsonPropertyName("used")]
    public double Used { get; set; }
}

public class AccountProxyBandwidth
{
    [JsonPropertyName("availableMb")]
    public double AvailableMb { get; set; }

    [JsonPropertyName("totalMb")]
    public double TotalMb { get; set; }

    [JsonPropertyName("usedMb")]
    public double UsedMb { get; set; }
}

public class AccountSummary
{
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("plan")]
    public AccountPlan? Plan { get; set; }

    [JsonPropertyName("credits")]
    public AccountCredits? Credits { get; set; }

    [JsonPropertyName("proxyBandwidth")]
    public AccountProxyBandwidth? ProxyBandwidth { get; set; }
}

public class UsageStatRow
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

public class UsageBreakdownRow
{
    [JsonPropertyName("date")]
    public required string Date { get; set; }

    [JsonPropertyName("tool")]
    public required string Tool { get; set; }

    [JsonPropertyName("credits")]
    public double Credits { get; set; }

    [JsonPropertyName("requests")]
    public int Requests { get; set; }
}

