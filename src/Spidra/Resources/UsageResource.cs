using Spidra.Http;
using Spidra.Types.Usage;

namespace Spidra.Resources;

public class UsageResource
{
    private readonly SpidraHttpClient _http;

    internal UsageResource(SpidraHttpClient http) => _http = http;

    /// <summary>Retrieve credit usage for the account.</summary>
    public Task<UsageResponse> GetAsync(CancellationToken cancellationToken = default)
        => _http.GetAsync<UsageResponse>("/account/usage", cancellationToken);
}
