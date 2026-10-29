using Spidra.Http;
using Spidra.Types.Account;

namespace Spidra.Resources;

public class AccountResource
{
    private readonly SpidraHttpClient _http;

    internal AccountResource(SpidraHttpClient http) => _http = http;

    /// <summary>Retrieve account plan details, credit balances, and residential proxy bandwidth limits.</summary>
    public Task<AccountSummary> GetAsync(CancellationToken cancellationToken = default)
        => _http.GetAsync<AccountSummary>("/account", null, cancellationToken);

    /// <summary>Retrieve credit and request consumption over time ("7d", "30d", "weekly").</summary>
    public Task<List<UsageStatRow>> UsageAsync(string range = "30d", CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?> { ["range"] = range };
        return _http.GetAsync<List<UsageStatRow>>("/usage-stats", query, cancellationToken);
    }

    /// <summary>Retrieve credit consumption broken down by tool ("1d", "7d", "30d", "custom").</summary>
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

