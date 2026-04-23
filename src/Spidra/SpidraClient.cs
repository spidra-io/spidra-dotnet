using Spidra.Http;
using Spidra.Resources;

namespace Spidra;

/// <summary>
/// Client for the Spidra web scraping API.
/// </summary>
/// <example>
/// <code>
/// var client = new SpidraClient("your-api-key");
/// var job = await client.Scrape.RunAsync(new ScrapeParams
/// {
///     Urls = [new ScrapeUrl("https://example.com")],
///     Prompt = "Extract the page title and main content"
/// });
/// </code>
/// </example>
public class SpidraClient
{
    private readonly SpidraHttpClient _http;

    /// <summary>Submit and retrieve individual scrape jobs.</summary>
    public ScrapeResource Scrape { get; }

    /// <summary>Submit and retrieve batch scrape jobs across multiple URLs.</summary>
    public BatchResource Batch { get; }

    /// <summary>Submit and retrieve site crawl jobs.</summary>
    public CrawlResource Crawl { get; }

    /// <summary>Retrieve scrape logs for the account.</summary>
    public LogsResource Logs { get; }

    /// <summary>Retrieve credit usage information.</summary>
    public UsageResource Usage { get; }

    /// <summary>
    /// Initialise a new Spidra client.
    /// </summary>
    /// <param name="apiKey">Your Spidra API key. Get one at spidra.io.</param>
    /// <param name="httpClient">
    /// Optional <see cref="HttpClient"/> to use. Useful for testing or when you need
    /// custom timeout/proxy settings. If omitted, a shared client is created.
    /// </param>
    public SpidraClient(string apiKey, HttpClient? httpClient = null)
    {
        _http = new SpidraHttpClient(apiKey, httpClient);
        Scrape = new ScrapeResource(_http);
        Batch = new BatchResource(_http);
        Crawl = new CrawlResource(_http);
        Logs = new LogsResource(_http);
        Usage = new UsageResource(_http);
    }
}
