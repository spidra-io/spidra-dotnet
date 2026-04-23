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

    /// <summary>Retrieve the current status of a scrape job by ID.</summary>
    public Task<ScrapeJobResponse> GetAsync(string jobId, CancellationToken cancellationToken = default)
        => _http.GetAsync<ScrapeJobResponse>($"/scrape/{jobId}", cancellationToken);

    /// <summary>Submit a scrape job and wait for it to complete, then return the result.</summary>
    public async Task<ScrapeJobResponse> RunAsync(ScrapeParams @params, CancellationToken cancellationToken = default)
    {
        var job = await SubmitAsync(@params, cancellationToken);
        if (job.Status is "completed" or "failed")
            return job;

        return await _http.PollAsync<ScrapeJobResponse>(
            $"/scrape/{job.JobId}",
            j => j.Status,
            cancellationToken);
    }
}
