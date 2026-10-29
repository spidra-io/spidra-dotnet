using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
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

    public SpidraHttpClient(string apiKey, string? baseUrl = null, HttpClient? httpClient = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

        _http = httpClient ?? new HttpClient();
        _http.BaseAddress ??= new Uri(baseUrl ?? DefaultBaseUrl);
        _http.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {apiKey}");
        _http.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", apiKey);
        _http.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "spidra-dotnet/0.2.0");
    }

    public async Task<T> PostAsync<T>(string path, object? body = null, CancellationToken cancellationToken = default)
    {
        var content = body is not null ? JsonContent.Create(body, options: JsonOptions) : null;
        var response = await _http.PostAsync(path, content, cancellationToken);
        await EnsureSuccessAsync(response);
        return (await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken))!;
    }

    public async Task PostAsync(string path, object? body = null, CancellationToken cancellationToken = default)
    {
        var content = body is not null ? JsonContent.Create(body, options: JsonOptions) : null;
        var response = await _http.PostAsync(path, content, cancellationToken);
        await EnsureSuccessAsync(response);
    }

    public async Task<T> GetAsync<T>(string path, IDictionary<string, string?>? query = null, CancellationToken cancellationToken = default)
    {
        var uri = BuildUriWithQuery(path, query);
        var response = await _http.GetAsync(uri, cancellationToken);
        await EnsureSuccessAsync(response);
        return (await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken))!;
    }

    public async Task<T> PatchAsync<T>(string path, object body, CancellationToken cancellationToken = default)
    {
        var response = await _http.PatchAsJsonAsync(path, body, JsonOptions, cancellationToken);
        await EnsureSuccessAsync(response);
        return (await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken))!;
    }

    public async Task<T> PutAsync<T>(string path, object body, CancellationToken cancellationToken = default)
    {
        var response = await _http.PutAsJsonAsync(path, body, JsonOptions, cancellationToken);
        await EnsureSuccessAsync(response);
        return (await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken))!;
    }

    public async Task<T> DeleteAsync<T>(string path, CancellationToken cancellationToken = default)
    {
        var response = await _http.DeleteAsync(path, cancellationToken);
        await EnsureSuccessAsync(response);
        return (await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken))!;
    }

    public async Task DeleteAsync(string path, CancellationToken cancellationToken = default)
    {
        var response = await _http.DeleteAsync(path, cancellationToken);
        await EnsureSuccessAsync(response);
    }

    public async Task<T> PollAsync<T>(
        string getPath,
        Func<T, string> getStatus,
        Func<T, string?>? getError = null,
        Func<T, string?>? getJobId = null,
        TimeSpan? timeout = null,
        TimeSpan? pollInterval = null,
        CancellationToken cancellationToken = default) where T : class
    {
        var effectiveTimeout = timeout ?? TimeSpan.FromSeconds(120);
        var effectiveInterval = pollInterval ?? TimeSpan.FromSeconds(2);
        var deadline = DateTimeOffset.UtcNow.Add(effectiveTimeout);

        string lastJobId = string.Empty;

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (DateTimeOffset.UtcNow > deadline)
            {
                throw new SpidraTimeoutException(
                    $"Spidra: Polling timed out after {effectiveTimeout.TotalSeconds}s for {getPath}",
                    lastJobId,
                    (int)effectiveTimeout.TotalSeconds);
            }

            await Task.Delay(effectiveInterval, cancellationToken);

            var result = await GetAsync<T>(getPath, null, cancellationToken);
            var status = getStatus(result)?.ToLowerInvariant();

            if (getJobId is not null)
            {
                lastJobId = getJobId(result) ?? lastJobId;
            }

            if (status is "completed")
            {
                return result;
            }

            if (status is "failed" or "error" or "cancelled")
            {
                var error = getError?.Invoke(result) ?? $"Job ended with status '{status}'";
                throw new SpidraJobFailedException(error, lastJobId, status);
            }
        }
    }

    private static string BuildUriWithQuery(string path, IDictionary<string, string?>? query)
    {
        if (query is null || query.Count == 0)
            return path;

        var q = HttpUtility.ParseQueryString(string.Empty);
        foreach (var (key, value) in query)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                q[key] = value;
            }
        }

        var queryString = q.ToString();
        return string.IsNullOrEmpty(queryString) ? path : $"{path}?{queryString}";
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            return;

        var statusCode = (int)response.StatusCode;
        string body = string.Empty;
        string errorMessage = string.Empty;
        List<string> details = [];

        try
        {
            body = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(body))
            {
                using var doc = JsonDocument.Parse(body);
                var root = doc.RootElement;
                if (root.TryGetProperty("error", out var errProp))
                {
                    errorMessage = errProp.GetString() ?? string.Empty;
                }
                else if (root.TryGetProperty("message", out var msgProp))
                {
                    errorMessage = msgProp.GetString() ?? string.Empty;
                }

                if (root.TryGetProperty("details", out var detProp) && detProp.ValueKind == JsonValueKind.Array)
                {
                    foreach (var elem in detProp.EnumerateArray())
                    {
                        var s = elem.GetString();
                        if (!string.IsNullOrWhiteSpace(s))
                            details.Add(s);
                    }
                }
            }
        }
        catch
        {
            // fallback to raw body
        }

        if (string.IsNullOrWhiteSpace(errorMessage))
        {
            errorMessage = !string.IsNullOrWhiteSpace(body) ? body : $"HTTP request failed with status code {statusCode}";
        }

        throw statusCode switch
        {
            400 when details.Count > 0 => new SpidraValidationException(errorMessage, details, 400, body),
            401 => new SpidraAuthenticationException(errorMessage, body),
            402 => new SpidraInsufficientCreditsException(errorMessage, body),
            403 => new SpidraForbiddenException(errorMessage, body),
            404 => new SpidraNotFoundException(errorMessage, body),
            422 => new SpidraValidationException(errorMessage, details, 422, body),
            429 => new SpidraRateLimitException(errorMessage, response.Headers.RetryAfter?.Delta, body),
            _ when statusCode >= 500 => new SpidraServerException(errorMessage, statusCode, body),
            _ => new SpidraException(errorMessage, statusCode, body)
        };
    }
}
