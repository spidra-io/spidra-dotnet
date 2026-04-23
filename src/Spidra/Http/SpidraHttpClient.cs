using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Spidra.Exceptions;

namespace Spidra.Http;

internal class SpidraHttpClient
{
    private const string DefaultBaseUrl = "https://api.spidra.io/api";

    private readonly HttpClient _http;

    internal static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public SpidraHttpClient(string apiKey, HttpClient? httpClient = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

        _http = httpClient ?? new HttpClient();
        _http.BaseAddress ??= new Uri(DefaultBaseUrl);
        _http.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", apiKey);
    }

    public async Task<T> PostAsync<T>(string path, object body, CancellationToken cancellationToken = default)
    {
        var response = await _http.PostAsJsonAsync(path, body, JsonOptions, cancellationToken);
        await EnsureSuccessAsync(response);
        return (await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken))!;
    }

    public async Task<T> GetAsync<T>(string path, CancellationToken cancellationToken = default)
    {
        var response = await _http.GetAsync(path, cancellationToken);
        await EnsureSuccessAsync(response);
        return (await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken))!;
    }

    public async Task PostAsync(string path, object body, CancellationToken cancellationToken = default)
    {
        var response = await _http.PostAsJsonAsync(path, body, JsonOptions, cancellationToken);
        await EnsureSuccessAsync(response);
    }

    public async Task<T> PollAsync<T>(
        string getPath,
        Func<T, string> getStatus,
        CancellationToken cancellationToken = default) where T : class
    {
        var delay = TimeSpan.FromSeconds(1);
        var maxDelay = TimeSpan.FromSeconds(5);

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay(delay, cancellationToken);
            delay = delay < maxDelay ? delay + TimeSpan.FromSeconds(1) : maxDelay;

            var result = await GetAsync<T>(getPath, cancellationToken);
            var status = getStatus(result);

            if (status is "completed" or "failed")
                return result;
        }
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            return;

        var statusCode = (int)response.StatusCode;
        string body;
        try { body = await response.Content.ReadAsStringAsync(); }
        catch { body = string.Empty; }

        throw statusCode switch
        {
            401 => new SpidraAuthenticationException(),
            402 => new SpidraInsufficientCreditsException(),
            429 => new SpidraRateLimitException(retryAfter: response.Headers.RetryAfter?.Delta),
            _ when statusCode >= 500 => new SpidraServerException($"Server error {statusCode}: {body}", statusCode),
            _ => new SpidraException($"Unexpected error {statusCode}: {body}", statusCode)
        };
    }
}
