using Spidra.Http;
using Spidra.Types.Logs;

namespace Spidra.Resources;

public class LogsResource
{
    private readonly SpidraHttpClient _http;

    internal LogsResource(SpidraHttpClient http) => _http = http;

    /// <summary>Retrieve scrape logs for the account.</summary>
    public Task<ScrapeLogsResponse> ScrapeAsync(LogsParams? @params = null, CancellationToken cancellationToken = default)
        => _http.GetAsync<ScrapeLogsResponse>("/scrape-logs", @params?.ToQueryDictionary(), cancellationToken);

    /// <summary>Alias for <see cref="ScrapeAsync"/>.</summary>
    public Task<ScrapeLogsResponse> ListAsync(LogsParams? @params = null, CancellationToken cancellationToken = default)
        => ScrapeAsync(@params, cancellationToken);

    /// <summary>Retrieve a single scrape log entry with full extraction data.</summary>
    public Task<ScrapeLogDetail> GetScrapeAsync(string uuid, CancellationToken cancellationToken = default)
        => _http.GetAsync<ScrapeLogDetail>($"/scrape-logs/{uuid}", null, cancellationToken);

    /// <summary>Alias for <see cref="GetScrapeAsync"/>.</summary>
    public Task<ScrapeLogDetail> GetAsync(string uuid, CancellationToken cancellationToken = default)
        => GetScrapeAsync(uuid, cancellationToken);

    /// <summary>Retrieve search query logs with optional filters.</summary>
    public Task<SearchLogsResponse> SearchAsync(LogsParams? @params = null, CancellationToken cancellationToken = default)
        => _http.GetAsync<SearchLogsResponse>("/search-logs", @params?.ToQueryDictionary(), cancellationToken);

    /// <summary>Retrieve details of a single search log entry.</summary>
    public Task<SearchLog> GetSearchAsync(string uuid, CancellationToken cancellationToken = default)
        => _http.GetAsync<SearchLog>($"/search-logs/{uuid}", null, cancellationToken);

    /// <summary>Retrieve domain mapping logs with optional filters.</summary>
    public Task<MapLogsResponse> MapAsync(LogsParams? @params = null, CancellationToken cancellationToken = default)
        => _http.GetAsync<MapLogsResponse>("/map-logs", @params?.ToQueryDictionary(), cancellationToken);

    /// <summary>Retrieve details of a single domain map log entry.</summary>
    public Task<MapLog> GetMapAsync(string uuid, CancellationToken cancellationToken = default)
        => _http.GetAsync<MapLog>($"/map-logs/{uuid}", null, cancellationToken);

    /// <summary>Retrieve document parsing logs with optional filters.</summary>
    public Task<ParseLogsResponse> ParseAsync(LogsParams? @params = null, CancellationToken cancellationToken = default)
        => _http.GetAsync<ParseLogsResponse>("/parse-logs", @params?.ToQueryDictionary(), cancellationToken);
}
