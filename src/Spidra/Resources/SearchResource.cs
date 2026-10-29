using Spidra.Http;
using Spidra.Types.Search;

namespace Spidra.Resources;

public class SearchResource
{
    private readonly SpidraHttpClient _http;

    internal SearchResource(SpidraHttpClient http) => _http = http;

    /// <summary>Submit a search job without waiting for it to complete.</summary>
    public Task<SearchJobResponse> SubmitAsync(SearchParams @params, CancellationToken cancellationToken = default)
        => _http.PostAsync<SearchJobResponse>("/search", @params, cancellationToken);

    /// <summary>Submit a search query string without waiting.</summary>
    public Task<SearchJobResponse> SubmitAsync(string query, CancellationToken cancellationToken = default)
        => SubmitAsync(SearchParams.ForQuery(query), cancellationToken);

    /// <summary>Retrieve the current status and results of a search job by ID.</summary>
    public Task<SearchJobResponse> GetAsync(string jobId, CancellationToken cancellationToken = default)
        => _http.GetAsync<SearchJobResponse>($"/search/{jobId}", null, cancellationToken);

    /// <summary>Cancel an in-progress search job.</summary>
    public Task CancelAsync(string jobId, CancellationToken cancellationToken = default)
        => _http.DeleteAsync($"/search/{jobId}", cancellationToken);

    /// <summary>Execute a search query and wait for completion.</summary>
    public async Task<SearchJobResponse> RunAsync(
        SearchParams @params,
        TimeSpan? timeout = null,
        TimeSpan? pollInterval = null,
        CancellationToken cancellationToken = default)
    {
        @params.Wait = true;
        var job = await SubmitAsync(@params, cancellationToken);

        if (job.Status is "completed")
            return job;
        if (job.Status is "failed" or "error" or "cancelled")
            throw new Exceptions.SpidraJobFailedException(job.Error ?? $"Search job ended with status {job.Status}", job.JobId, job.Status);

        return await _http.PollAsync<SearchJobResponse>(
            $"/search/{job.JobId}",
            j => j.Status,
            j => j.Error,
            j => j.JobId,
            timeout,
            pollInterval,
            cancellationToken);
    }

    /// <summary>Search with a query string and wait for results.</summary>
    public Task<SearchJobResponse> RunQueryAsync(
        string query,
        TimeSpan? timeout = null,
        CancellationToken cancellationToken = default)
        => RunAsync(SearchParams.ForQuery(query), timeout, null, cancellationToken);
}

