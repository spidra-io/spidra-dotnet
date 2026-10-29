using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spidra.Types.Monitors;

public class MonitorItem
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("schedule")]
    public required string Schedule { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; } // active, paused

    [JsonPropertyName("selector")]
    public string? Selector { get; set; }

    [JsonPropertyName("condition")]
    public string? Condition { get; set; }

    [JsonPropertyName("channels")]
    public Dictionary<string, object?>? Channels { get; set; }

    [JsonPropertyName("lastCheckedAt")]
    public string? LastCheckedAt { get; set; }

    [JsonPropertyName("lastChangedAt")]
    public string? LastChangedAt { get; set; }

    [JsonPropertyName("createdAt")]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public string? UpdatedAt { get; set; }
}

public class MonitorParams
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("schedule")]
    public required string Schedule { get; set; }

    [JsonPropertyName("selector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Selector { get; set; }

    [JsonPropertyName("condition")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Condition { get; set; }

    [JsonPropertyName("channels")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object?>? Channels { get; set; }

    [JsonPropertyName("useProxy")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool UseProxy { get; set; }

    [JsonPropertyName("proxyCountry")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ProxyCountry { get; set; }
}

public class MonitorEstimate
{
    [JsonPropertyName("estimatedMonthlyCredits")]
    public double EstimatedMonthlyCredits { get; set; }

    [JsonPropertyName("checksPerMonth")]
    public int ChecksPerMonth { get; set; }

    [JsonPropertyName("costPerCheck")]
    public double CostPerCheck { get; set; }
}

public class MonitorLimits
{
    [JsonPropertyName("activeMonitors")]
    public int ActiveMonitors { get; set; }

    [JsonPropertyName("maxMonitors")]
    public int MaxMonitors { get; set; }

    [JsonPropertyName("minimumIntervalMinutes")]
    public int MinimumIntervalMinutes { get; set; }
}

public class NotificationTestResult
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("channels")]
    public Dictionary<string, bool> Channels { get; set; } = [];

    [JsonPropertyName("errors")]
    public Dictionary<string, string> Errors { get; set; } = [];
}

public class MonitorSourceChanges
{
    [JsonPropertyName("previousContent")]
    public string? PreviousContent { get; set; }

    [JsonPropertyName("currentContent")]
    public string? CurrentContent { get; set; }

    [JsonPropertyName("extractedChanges")]
    public JsonElement? ExtractedChanges { get; set; }
}

public class MonitorCheck
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("monitorId")]
    public required string MonitorId { get; set; }

    [JsonPropertyName("checkedAt")]
    public required string CheckedAt { get; set; }

    [JsonPropertyName("hasChanges")]
    public bool HasChanges { get; set; }

    [JsonPropertyName("diff")]
    public string? Diff { get; set; }

    [JsonPropertyName("changes")]
    public MonitorSourceChanges? Changes { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }
}

public class ListMonitorsResponse
{
    [JsonPropertyName("monitors")]
    public List<MonitorItem> Monitors { get; set; } = [];

    [JsonPropertyName("total")]
    public int Total { get; set; }
}

public class ListMonitorChecksResponse
{
    [JsonPropertyName("checks")]
    public List<MonitorCheck> Checks { get; set; } = [];

    [JsonPropertyName("total")]
    public int Total { get; set; }
}

