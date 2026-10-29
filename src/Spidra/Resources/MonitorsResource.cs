using Spidra.Http;
using Spidra.Types.Monitors;

namespace Spidra.Resources;

public class MonitorsResource
{
    private readonly SpidraHttpClient _http;

    internal MonitorsResource(SpidraHttpClient http) => _http = http;

    /// <summary>List scheduled change monitors with optional pagination.</summary>
    public Task<ListMonitorsResponse> ListAsync(int? page = null, int? limit = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (page.HasValue) query["page"] = page.Value.ToString();
        if (limit.HasValue) query["limit"] = limit.Value.ToString();
        return _http.GetAsync<ListMonitorsResponse>("/monitors", query, cancellationToken);
    }

    /// <summary>Retrieve a scheduled monitor by ID.</summary>
    public Task<MonitorItem> GetAsync(string monitorId, CancellationToken cancellationToken = default)
        => _http.GetAsync<MonitorItem>($"/monitors/{monitorId}", null, cancellationToken);

    /// <summary>Create a new scheduled website change monitor.</summary>
    public Task<MonitorItem> CreateAsync(MonitorParams @params, CancellationToken cancellationToken = default)
        => _http.PostAsync<MonitorItem>("/monitors", @params, cancellationToken);

    /// <summary>Update an existing scheduled monitor configuration.</summary>
    public Task<MonitorItem> UpdateAsync(string monitorId, object updates, CancellationToken cancellationToken = default)
        => _http.PatchAsync<MonitorItem>($"/monitors/{monitorId}", updates, cancellationToken);

    /// <summary>Delete a scheduled monitor by ID.</summary>
    public Task DeleteAsync(string monitorId, CancellationToken cancellationToken = default)
        => _http.DeleteAsync($"/monitors/{monitorId}", cancellationToken);

    /// <summary>Estimate monthly credit consumption for a monitor schedule.</summary>
    public Task<MonitorEstimate> EstimateAsync(object paramsObj, CancellationToken cancellationToken = default)
        => _http.PostAsync<MonitorEstimate>("/monitors/estimate", paramsObj, cancellationToken);

    /// <summary>Retrieve active monitor limits and current account tier usage.</summary>
    public Task<MonitorLimits> LimitsAsync(CancellationToken cancellationToken = default)
        => _http.GetAsync<MonitorLimits>("/monitors/limits", null, cancellationToken);

    /// <summary>Send a test notification to configured alert channels (webhook, email, slack).</summary>
    public Task<NotificationTestResult> TestNotificationAsync(
        Dictionary<string, object?> channels,
        object? sampleData = null,
        CancellationToken cancellationToken = default)
    {
        var body = new
        {
            channels,
            sampleData
        };
        return _http.PostAsync<NotificationTestResult>("/monitors/test-notification", body, cancellationToken);
    }

    /// <summary>Trigger an immediate out-of-schedule check for a monitor.</summary>
    public Task<Dictionary<string, object?>> CheckNowAsync(string monitorId, CancellationToken cancellationToken = default)
        => _http.PostAsync<Dictionary<string, object?>>($"/monitors/{monitorId}/check-now", new { }, cancellationToken);

    /// <summary>Pause scheduled checks for a monitor.</summary>
    public Task<MonitorItem> PauseAsync(string monitorId, CancellationToken cancellationToken = default)
        => _http.PostAsync<MonitorItem>($"/monitors/{monitorId}/pause", new { }, cancellationToken);

    /// <summary>Resume scheduled checks for a paused monitor.</summary>
    public Task<MonitorItem> ResumeAsync(string monitorId, CancellationToken cancellationToken = default)
        => _http.PostAsync<MonitorItem>($"/monitors/{monitorId}/resume", new { }, cancellationToken);

    /// <summary>List historical check runs for a monitor with pagination.</summary>
    public Task<ListMonitorChecksResponse> ListChecksAsync(
        string monitorId,
        int? page = null,
        int? limit = null,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (page.HasValue) query["page"] = page.Value.ToString();
        if (limit.HasValue) query["limit"] = limit.Value.ToString();
        return _http.GetAsync<ListMonitorChecksResponse>($"/monitors/{monitorId}/checks", query, cancellationToken);
    }

    /// <summary>Retrieve details and diff of a specific historical check run.</summary>
    public Task<MonitorCheck> GetCheckAsync(string monitorId, string checkId, CancellationToken cancellationToken = default)
        => _http.GetAsync<MonitorCheck>($"/monitors/{monitorId}/checks/{checkId}", null, cancellationToken);
}

