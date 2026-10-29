using Spidra.Http;
using Spidra.Types.Scrape;

namespace Spidra.Resources;

public class ScrapeResource
{
    private readonly SpidraHttpClient _http;

    internal ScrapeResource(SpidraHttpClient http) => _http = http;

    /// <summary>Submit a scrape job without waiting for it to complete.</summary>
    public Task<ScrapeJobResponse> SubmitAsync(ScrapeParams @params, CancellationToken cancellationToken = default)
        => _http.PostAsync<ScrapeJobResponse>("/scrape", @params, cancellationToken);

    /// <summary>Submit a single URL scrape job without waiting.</summary>
    public Task<ScrapeJobResponse> SubmitAsync(string url, CancellationToken cancellationToken = default)
        => SubmitAsync(ScrapeParams.ForUrl(url), cancellationToken);

    /// <summary>Retrieve the current status of a scrape job by ID.</summary>
    public Task<ScrapeJobResponse> GetAsync(string jobId, CancellationToken cancellationToken = default)
        => _http.GetAsync<ScrapeJobResponse>($"/scrape/{jobId}", null, cancellationToken);

    /// <summary>Submit a scrape job and wait for it to complete, then return the result.</summary>
    public async Task<ScrapeJobResponse> RunAsync(
        ScrapeParams @params,
        TimeSpan? timeout = null,
        TimeSpan? pollInterval = null,
        CancellationToken cancellationToken = default)
    {
        var job = await SubmitAsync(@params, cancellationToken);
        if (job.Status is "completed")
            return job;
        if (job.Status is "failed" or "error" or "cancelled")
            throw new Exceptions.SpidraJobFailedException(job.Error ?? $"Scrape job ended with status {job.Status}", job.JobId, job.Status);

        return await _http.PollAsync<ScrapeJobResponse>(
            $"/scrape/{job.JobId}",
            j => j.Status,
            j => j.Error,
            j => j.JobId,
            timeout,
            pollInterval,
            cancellationToken);
    }

    /// <summary>Convenience helper to scrape a URL and wait for completion.</summary>
    public Task<ScrapeJobResponse> RunAsync(
        string url,
        string? prompt = null,
        OutputFormat? output = null,
        TimeSpan? timeout = null,
        CancellationToken cancellationToken = default)
        => RunAsync(ScrapeParams.ForUrl(url, prompt, output), timeout, null, cancellationToken);
}
