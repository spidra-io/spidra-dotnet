using Spidra.Http;
using Spidra.Types.Batch;
using Spidra.Types.Scrape;

namespace Spidra.Resources;

public class BatchResource
{
    private readonly SpidraHttpClient _http;

    internal BatchResource(SpidraHttpClient http) => _http = http;

    /// <summary>Submit a batch scrape job without waiting for it to complete.</summary>
    public Task<BatchJobResponse> SubmitAsync(BatchScrapeParams @params, CancellationToken cancellationToken = default)
        => _http.PostAsync<BatchJobResponse>("/batch/scrape", @params, cancellationToken);

    /// <summary>Submit a list of URLs for batch scraping without waiting.</summary>
    public Task<BatchJobResponse> SubmitAsync(
        List<string> urls,
        string? prompt = null,
        OutputFormat? output = null,
        CancellationToken cancellationToken = default)
        => SubmitAsync(BatchScrapeParams.ForUrls(urls, prompt, output), cancellationToken);

    /// <summary>Retrieve the current status of a batch job by ID.</summary>
    public Task<BatchJobResponse> GetAsync(string batchId, CancellationToken cancellationToken = default)
        => _http.GetAsync<BatchJobResponse>($"/batch/scrape/{batchId}", null, cancellationToken);

    /// <summary>Submit a batch scrape job and wait for all items to finish, then return the result.</summary>
    public async Task<BatchJobResponse> RunAsync(
        BatchScrapeParams @params,
        TimeSpan? timeout = null,
        TimeSpan? pollInterval = null,
        CancellationToken cancellationToken = default)
    {
        var batch = await SubmitAsync(@params, cancellationToken);
        if (batch.Status is "completed")
            return batch;
        if (batch.Status is "failed" or "error" or "cancelled")
            throw new Exceptions.SpidraJobFailedException($"Batch scrape ended with status {batch.Status}: failed {batch.FailedCount} of {batch.TotalUrls}", batch.BatchId, batch.Status);

        return await _http.PollAsync<BatchJobResponse>(
            $"/batch/scrape/{batch.BatchId}",
            b => b.Status,
            b => $"Batch scrape ended with status {b.Status}: failed {b.FailedCount} of {b.TotalUrls}",
            b => b.BatchId,
            timeout,
            pollInterval,
            cancellationToken);
    }

    /// <summary>Batch scrape a list of URLs and wait for completion.</summary>
    public Task<BatchJobResponse> RunAsync(
        List<string> urls,
        string? prompt = null,
        OutputFormat? output = null,
        TimeSpan? timeout = null,
        CancellationToken cancellationToken = default)
        => RunAsync(BatchScrapeParams.ForUrls(urls, prompt, output), timeout, null, cancellationToken);

    /// <summary>Retry failed items in a completed batch with optional session cookies.</summary>
    public Task RetryAsync(string batchId, string? cookies = null, CancellationToken cancellationToken = default)
    {
        var body = !string.IsNullOrWhiteSpace(cookies) ? (object)new { cookies } : new { };
        return _http.PostAsync($"/batch/scrape/{batchId}/retry", body, cancellationToken);
    }

    /// <summary>Cancel a running batch job and request refunds for unprocessed URLs.</summary>
    public Task<BatchCancelResult> CancelAsync(string batchId, CancellationToken cancellationToken = default)
        => _http.DeleteAsync<BatchCancelResult>($"/batch/scrape/{batchId}", cancellationToken);

    /// <summary>List past batch scrape jobs with optional pagination.</summary>
    public Task<BatchListResponse> ListAsync(int? page = null, int? limit = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (page.HasValue) query["page"] = page.Value.ToString();
        if (limit.HasValue) query["limit"] = limit.Value.ToString();
        return _http.GetAsync<BatchListResponse>("/batch/scrape", query, cancellationToken);
    }
}
