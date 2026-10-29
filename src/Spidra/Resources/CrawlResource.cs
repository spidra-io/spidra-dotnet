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

    /// <summary>Submit a crawl job for a seed URL without waiting.</summary>
    public Task<CrawlJobResponse> SubmitAsync(string url, CancellationToken cancellationToken = default)
        => SubmitAsync(CrawlParams.ForUrl(url), cancellationToken);

    /// <summary>Retrieve the current status of a crawl job by ID.</summary>
    public Task<CrawlJobResponse> GetAsync(string jobId, CancellationToken cancellationToken = default)
        => _http.GetAsync<CrawlJobResponse>($"/crawl/{jobId}", null, cancellationToken);

    /// <summary>Submit a crawl job and wait for it to complete, then return the result.</summary>
    public async Task<CrawlJobResponse> RunAsync(
        CrawlParams @params,
        TimeSpan? timeout = null,
        TimeSpan? pollInterval = null,
        CancellationToken cancellationToken = default)
    {
        var job = await SubmitAsync(@params, cancellationToken);
        if (job.Status is "completed")
            return job;
        if (job.Status is "failed" or "error" or "cancelled")
            throw new Exceptions.SpidraJobFailedException(job.Error ?? $"Crawl job ended with status {job.Status}", job.JobId, job.Status);

        return await _http.PollAsync<CrawlJobResponse>(
            $"/crawl/{job.JobId}",
            j => j.Status,
            j => j.Error,
            j => j.JobId,
            timeout ?? TimeSpan.FromMinutes(15),
            pollInterval ?? TimeSpan.FromSeconds(3),
            cancellationToken);
    }

    /// <summary>Crawl a seed URL and wait for completion.</summary>
    public Task<CrawlJobResponse> RunUrlAsync(
        string url,
        string? crawlInstruction = null,
        string? transformInstruction = null,
        TimeSpan? timeout = null,
        CancellationToken cancellationToken = default)
        => RunAsync(CrawlParams.ForUrl(url, crawlInstruction, transformInstruction), timeout, null, cancellationToken);

    /// <summary>Retrieve all collected pages for a completed crawl job, including signed download URLs.</summary>
    public Task<CrawlPagesResponse> PagesAsync(string jobId, CancellationToken cancellationToken = default)
        => _http.GetAsync<CrawlPagesResponse>($"/crawl/{jobId}/pages", null, cancellationToken);

    /// <summary>Re-run AI extraction on an existing crawl with a new natural language instruction.</summary>
    public Task<CrawlExtractResponse> ExtractAsync(string jobId, string transformInstruction, CancellationToken cancellationToken = default)
        => _http.PostAsync<CrawlExtractResponse>($"/crawl/{jobId}/extract", new { transformInstruction }, cancellationToken);

    /// <summary>Cancel an active crawl job and refund credits for unvisited pages.</summary>
    public Task<CrawlCancelResult> CancelAsync(string jobId, CancellationToken cancellationToken = default)
        => _http.DeleteAsync<CrawlCancelResult>($"/crawl/{jobId}", cancellationToken);

    /// <summary>Retrieve a diagnostic report detailing skipped URLs and crawl error codes.</summary>
    public Task<CrawlReportResponse> ReportAsync(string jobId, CancellationToken cancellationToken = default)
        => _http.GetAsync<CrawlReportResponse>($"/crawl/{jobId}/report", null, cancellationToken);

    /// <summary>Retry crawling a specific failed page in a crawl job.</summary>
    public Task<CrawlRetryPageResponse> RetryPageAsync(string jobId, string pageId, CancellationToken cancellationToken = default)
        => _http.PostAsync<CrawlRetryPageResponse>($"/crawl/{jobId}/pages/{pageId}/retry", new { }, cancellationToken);

    /// <summary>List past crawl jobs with optional pagination.</summary>
    public Task<CrawlHistory> HistoryAsync(int? page = null, int? limit = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (page.HasValue) query["page"] = page.Value.ToString();
        if (limit.HasValue) query["limit"] = limit.Value.ToString();
        return _http.GetAsync<CrawlHistory>("/crawl/history", query, cancellationToken);
    }

    /// <summary>Retrieve aggregate account crawl statistics.</summary>
    public Task<CrawlStats> StatsAsync(CancellationToken cancellationToken = default)
        => _http.GetAsync<CrawlStats>("/crawl/stats", null, cancellationToken);
}
