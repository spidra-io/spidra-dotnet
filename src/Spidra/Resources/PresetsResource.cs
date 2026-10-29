using Spidra.Http;
using Spidra.Types.Presets;

namespace Spidra.Resources;

public class PresetsResource
{
    private readonly SpidraHttpClient _http;

    internal PresetsResource(SpidraHttpClient http) => _http = http;

    /// <summary>List saved presets with optional pagination.</summary>
    public Task<ListPresetsResponse> ListAsync(int? page = null, int? limit = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (page.HasValue) query["page"] = page.Value.ToString();
        if (limit.HasValue) query["limit"] = limit.Value.ToString();
        return _http.GetAsync<ListPresetsResponse>("/presets", query, cancellationToken);
    }

    /// <summary>Retrieve a saved preset configuration by ID.</summary>
    public Task<Preset> GetAsync(string presetId, CancellationToken cancellationToken = default)
        => _http.GetAsync<Preset>($"/presets/{presetId}", null, cancellationToken);

    /// <summary>Create and save a new preset configuration.</summary>
    public Task<Preset> CreateAsync(PresetParams @params, CancellationToken cancellationToken = default)
        => _http.PostAsync<Preset>("/presets", @params, cancellationToken);

    /// <summary>Update an existing preset configuration.</summary>
    public Task<Preset> UpdateAsync(string presetId, object updates, CancellationToken cancellationToken = default)
        => _http.PatchAsync<Preset>($"/presets/{presetId}", updates, cancellationToken);

    /// <summary>Delete a preset by ID.</summary>
    public Task DeleteAsync(string presetId, CancellationToken cancellationToken = default)
        => _http.DeleteAsync($"/presets/{presetId}", cancellationToken);

    /// <summary>Execute a saved preset with optional runtime overrides and wait mode.</summary>
    public Task<RunPresetResponse> RunAsync(
        string presetId,
        Dictionary<string, object?>? overrides = null,
        bool wait = true,
        CancellationToken cancellationToken = default)
    {
        var body = new
        {
            overrides = overrides ?? [],
            wait
        };
        return _http.PostAsync<RunPresetResponse>($"/presets/{presetId}/run", body, cancellationToken);
    }
}

