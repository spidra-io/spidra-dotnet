using Spidra.Http;
using Spidra.Types.Map;

namespace Spidra.Resources;

public class MapResource
{
    private readonly SpidraHttpClient _http;

    internal MapResource(SpidraHttpClient http) => _http = http;

    /// <summary>Map a website starting from a root URL with parameters.</summary>
    public Task<MapResult> RunAsync(MapParams @params, CancellationToken cancellationToken = default)
        => _http.PostAsync<MapResult>("/map", @params, cancellationToken);

    /// <summary>Map a website starting from a root URL with optional keyword search filter.</summary>
    public Task<MapResult> RunUrlAsync(string url, string? search = null, CancellationToken cancellationToken = default)
        => RunAsync(MapParams.ForUrl(url, search), cancellationToken);
}

