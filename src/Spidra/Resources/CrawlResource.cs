using Spidra.Http;
using Spidra.Types.Crawl;

namespace Spidra.Resources;

public class CrawlResource
{
    private readonly SpidraHttpClient _http;

    internal CrawlResource(SpidraHttpClient http) => _http = http;

    /// <summary>Submit a crawl job without waiting for it to complete.</summary>
    public Task<CrawlJobResponse> SubmitAsync(CrawlParams @params, CancellationToken cancellationToken = default)
        => _http.PostAsync<CrawlJobResponse>("/crawl", @params, cancellationToken);

    /// <summary>Retrieve the current status of a crawl job by ID.</summary>
    public Task<CrawlJobResponse> GetAsync(string jobId, CancellationToken cancellationToken = default)
        => _http.GetAsync<CrawlJobResponse>($"/crawl/{jobId}", cancellationToken);

    /// <summary>Submit a crawl job and wait for it to complete, then return the result.</summary>
    public async Task<CrawlJobResponse> RunAsync(CrawlParams @params, CancellationToken cancellationToken = default)
    {
        var job = await SubmitAsync(@params, cancellationToken);
        if (job.Status is "completed" or "failed")
            return job;

        return await _http.PollAsync<CrawlJobResponse>(
            $"/crawl/{job.JobId}",
            j => j.Status,
            cancellationToken);
    }
}
