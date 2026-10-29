using Spidra.Http;
using Spidra.Types.Parse;

namespace Spidra.Resources;

public class ParseResource
{
    private readonly SpidraHttpClient _http;

    internal ParseResource(SpidraHttpClient http) => _http = http;

    /// <summary>Submit a document parse job without waiting.</summary>
    public Task<ParseJobResponse> SubmitAsync(ParseParams @params, CancellationToken cancellationToken = default)
        => _http.PostAsync<ParseJobResponse>("/parse", @params, cancellationToken);

    /// <summary>Submit a document URL for parsing without waiting.</summary>
    public Task<ParseJobResponse> SubmitAsync(string url, CancellationToken cancellationToken = default)
        => SubmitAsync(ParseParams.ForUrl(url), cancellationToken);

    /// <summary>Retrieve the current status and extracted content of a parse job by ID.</summary>
    public Task<ParseJobResponse> GetAsync(string jobId, CancellationToken cancellationToken = default)
        => _http.GetAsync<ParseJobResponse>($"/parse/{jobId}", null, cancellationToken);

    /// <summary>Parse a document and wait for extracted markdown, text, or structured data.</summary>
    public async Task<ParseJobResponse> RunAsync(
        ParseParams @params,
        TimeSpan? timeout = null,
        TimeSpan? pollInterval = null,
        CancellationToken cancellationToken = default)
    {
        @params.Wait = true;
        var job = await SubmitAsync(@params, cancellationToken);

        if (job.Status is "completed")
            return job;
        if (job.Status is "failed" or "error" or "cancelled")
            throw new Exceptions.SpidraJobFailedException(job.Error ?? $"Parse job ended with status {job.Status}", job.JobId, job.Status);

        return await _http.PollAsync<ParseJobResponse>(
            $"/parse/{job.JobId}",
            j => j.Status,
            j => j.Error,
            j => j.JobId,
            timeout,
            pollInterval,
            cancellationToken);
    }

    /// <summary>Parse a document URL and wait for completion.</summary>
    public Task<ParseJobResponse> RunUrlAsync(
        string url,
        string? mode = null,
        TimeSpan? timeout = null,
        CancellationToken cancellationToken = default)
        => RunAsync(ParseParams.ForUrl(url, mode), timeout, null, cancellationToken);

    /// <summary>Estimate credit costs for document parsing.</summary>
    public Task<ParseEstimate> EstimateAsync(ParseParams @params, CancellationToken cancellationToken = default)
        => _http.PostAsync<ParseEstimate>("/parse/estimate", @params, cancellationToken);

    /// <summary>Estimate credit costs for parsing a document URL.</summary>
    public Task<ParseEstimate> EstimateUrlAsync(string url, CancellationToken cancellationToken = default)
        => EstimateAsync(ParseParams.ForUrl(url), cancellationToken);

    /// <summary>Delete an uploaded parse file by its ID.</summary>
    public Task DeleteFileAsync(string fileId, CancellationToken cancellationToken = default)
        => _http.DeleteAsync($"/parse/files/{fileId}", cancellationToken);

    /// <summary>Submit a multi-document parsing batch.</summary>
    public Task<ParseBatchResponse> SubmitBatchAsync(object batchParams, CancellationToken cancellationToken = default)
        => _http.PostAsync<ParseBatchResponse>("/parse/batch", batchParams, cancellationToken);

    /// <summary>Retrieve the current status of a parse batch by ID.</summary>
    public Task<ParseBatchResponse> GetBatchAsync(string batchId, CancellationToken cancellationToken = default)
        => _http.GetAsync<ParseBatchResponse>($"/parse/batch/{batchId}", null, cancellationToken);
}

