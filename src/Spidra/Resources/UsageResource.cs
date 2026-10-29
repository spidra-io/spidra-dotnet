using Spidra.Http;
using Spidra.Types.Account;
using Spidra.Types.Usage;

namespace Spidra.Resources;

public class UsageResource
{
    private readonly SpidraHttpClient _http;

    internal UsageResource(SpidraHttpClient http) => _http = http;

    /// <summary>Retrieve credit and request usage statistics ("7d", "30d", "weekly").</summary>
    public Task<UsageResponse> GetAsync(string range = "30d", CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?> { ["range"] = range };
        return _http.GetAsync<UsageResponse>("/usage-stats", query, cancellationToken);
    }

    /// <summary>Retrieve tool-by-tool credit breakdown ("1d", "7d", "30d", "custom").</summary>
    public Task<List<UsageBreakdownRow>> BreakdownAsync(
        string range = "7d",
        string? startDate = null,
        string? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?> { ["range"] = range };
        if (!string.IsNullOrWhiteSpace(startDate)) query["startDate"] = startDate;
        if (!string.IsNullOrWhiteSpace(endDate)) query["endDate"] = endDate;
        return _http.GetAsync<List<UsageBreakdownRow>>("/usage-stats/breakdown", query, cancellationToken);
    }
}
