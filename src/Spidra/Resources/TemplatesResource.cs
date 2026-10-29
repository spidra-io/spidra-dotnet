using Spidra.Http;
using Spidra.Types.Templates;

namespace Spidra.Resources;

public class TemplatesResource
{
    private readonly SpidraHttpClient _http;

    internal TemplatesResource(SpidraHttpClient http) => _http = http;

    /// <summary>List available turnkey extraction templates with optional filters (category, site, search).</summary>
    public Task<ListTemplatesResponse> ListAsync(IDictionary<string, string?>? query = null, CancellationToken cancellationToken = default)
        => _http.GetAsync<ListTemplatesResponse>("/templates", query, cancellationToken);

    /// <summary>Retrieve the full specification and schema for a template by slug.</summary>
    public Task<TemplateDetail> GetAsync(string slug, CancellationToken cancellationToken = default)
        => _http.GetAsync<TemplateDetail>($"/templates/{slug}", null, cancellationToken);

    /// <summary>Estimate credit costs for running a template with the given inputs.</summary>
    public Task<TemplateEstimate> EstimateAsync(string slug, Dictionary<string, object?>? inputs = null, CancellationToken cancellationToken = default)
    {
        var body = new { inputs = inputs ?? [] };
        return _http.PostAsync<TemplateEstimate>($"/templates/{slug}/estimate", body, cancellationToken);
    }

    /// <summary>Submit an asynchronous template run without waiting.</summary>
    public Task<TemplateRunResponse> SubmitRunAsync(string slug, Dictionary<string, object?>? inputs = null, CancellationToken cancellationToken = default)
    {
        var body = new { inputs = inputs ?? [] };
        return _http.PostAsync<TemplateRunResponse>($"/templates/{slug}/run", body, cancellationToken);
    }

    /// <summary>Retrieve the current status and extracted rows for a template run.</summary>
    public Task<TemplateRunResponse> GetRunAsync(string slug, string runId, CancellationToken cancellationToken = default)
        => _http.GetAsync<TemplateRunResponse>($"/templates/{slug}/runs/{runId}", null, cancellationToken);

    /// <summary>Execute a turnkey extraction template and wait for completion.</summary>
    public async Task<TemplateRunResponse> RunAsync(
        string slug,
        Dictionary<string, object?>? inputs = null,
        TimeSpan? timeout = null,
        TimeSpan? pollInterval = null,
        CancellationToken cancellationToken = default)
    {
        var body = new
        {
            inputs = inputs ?? [],
            wait = true
        };

        var initial = await _http.PostAsync<TemplateRunResponse>($"/templates/{slug}/run", body, cancellationToken);
        if (initial.Status is "completed")
            return initial;
        if (initial.Status is "failed" or "cancelled" or "error")
            throw new Exceptions.SpidraJobFailedException(initial.Error ?? $"Template run ended with status {initial.Status}", initial.RunId, initial.Status);

        return await _http.PollAsync<TemplateRunResponse>(
            $"/templates/{slug}/runs/{initial.RunId}",
            r => r.Status,
            r => r.Error,
            r => r.RunId,
            timeout ?? TimeSpan.FromMinutes(5),
            pollInterval ?? TimeSpan.FromSeconds(3),
            cancellationToken);
    }

    /// <summary>List past executions for a specific template.</summary>
    public Task<ListTemplateRunsResponse> ListRunsAsync(string slug, int? page = null, int? limit = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (page.HasValue) query["page"] = page.Value.ToString();
        if (limit.HasValue) query["limit"] = limit.Value.ToString();
        return _http.GetAsync<ListTemplateRunsResponse>($"/templates/{slug}/runs", query, cancellationToken);
    }

    /// <summary>Search the airport directory by name, city, or IATA code.</summary>
    public Task<List<Airport>> LookupAirportsAsync(string query, CancellationToken cancellationToken = default)
    {
        var q = new Dictionary<string, string?> { ["q"] = query };
        return _http.GetAsync<List<Airport>>("/templates/lookup/airports", q, cancellationToken);
    }
}

