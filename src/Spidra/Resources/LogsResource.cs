using System.Web;
using Spidra.Http;
using Spidra.Types.Logs;

namespace Spidra.Resources;

public class LogsResource
{
    private readonly SpidraHttpClient _http;

    internal LogsResource(SpidraHttpClient http) => _http = http;

    /// <summary>Retrieve scrape logs for the account.</summary>
    public Task<List<ScrapeLog>> GetAsync(LogsParams? @params = null, CancellationToken cancellationToken = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);

        if (@params?.Limit.HasValue == true)
            query["limit"] = @params.Limit.Value.ToString();
        if (@params?.Offset.HasValue == true)
            query["offset"] = @params.Offset.Value.ToString();
        if (@params?.Status is not null)
            query["status"] = @params.Status;

        var qs = query.Count > 0 ? $"?{query}" : string.Empty;
        return _http.GetAsync<List<ScrapeLog>>($"/scrape-logs{qs}", cancellationToken);
    }
}
