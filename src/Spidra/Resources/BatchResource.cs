using Spidra.Http;
using Spidra.Types.Batch;

namespace Spidra.Resources;

public class BatchResource
{
    private readonly SpidraHttpClient _http;

    internal BatchResource(SpidraHttpClient http) => _http = http;

    /// <summary>Submit a batch scrape job without waiting for it to complete.</summary>
    public Task<BatchJobResponse> SubmitAsync(BatchScrapeParams @params, CancellationToken cancellationToken = default)
        => _http.PostAsync<BatchJobResponse>("/batch/scrape", @params, cancellationToken);

    /// <summary>Retrieve the current status of a batch job by ID.</summary>
    public Task<BatchJobResponse> GetAsync(string batchId, CancellationToken cancellationToken = default)
        => _http.GetAsync<BatchJobResponse>($"/batch/scrape/{batchId}", cancellationToken);

    /// <summary>Submit a batch scrape job and wait for all items to finish, then return the result.</summary>
    public async Task<BatchJobResponse> RunAsync(BatchScrapeParams @params, CancellationToken cancellationToken = default)
    {
        var batch = await SubmitAsync(@params, cancellationToken);
        if (batch.Status is "completed" or "failed")
            return batch;

        return await _http.PollAsync<BatchJobResponse>(
            $"/batch/scrape/{batch.BatchId}",
            b => b.Status,
            cancellationToken);
    }

    /// <summary>Retry failed items in a completed batch.</summary>
    public Task RetryAsync(string batchId, CancellationToken cancellationToken = default)
        => _http.PostAsync($"/batch/scrape/{batchId}/retry", new { }, cancellationToken);
}
