using Spidra.Http;
using Spidra.Resources;
using Spidra.Types.Account;
using Spidra.Types.Batch;
using Spidra.Types.Crawl;
using Spidra.Types.Map;
using Spidra.Types.Parse;
using Spidra.Types.Presets;
using Spidra.Types.Scrape;
using Spidra.Types.Search;
using Spidra.Types.Templates;

namespace Spidra;

/// <summary>
/// Client for the Spidra API — AI web scraping, search, document parsing, site crawling, templates, presets, and monitors.
/// </summary>
/// <example>
/// <code>
/// var client = new SpidraClient("your-api-key");
/// var job = await client.Scrape.RunAsync("https://example.com", "Extract pricing tiers");
/// </code>
/// </example>
public class SpidraClient
{
    private readonly SpidraHttpClient _http;

    /// <summary>Submit and retrieve individual and multi-URL scrape jobs with browser actions.</summary>
    public ScrapeResource Scrape { get; }

    /// <summary>Submit multimodal search queries across web, news, developer, and research sources.</summary>
    public SearchResource Search { get; }

    /// <summary>Discover all URLs on a domain using sitemaps and site graphs.</summary>
    public MapResource Map { get; }

    /// <summary>Parse PDFs, Office documents, audio, and video files with AI or fast layout modes.</summary>
    public ParseResource Parse { get; }

    /// <summary>Submit and retrieve recursive site crawl jobs.</summary>
    public CrawlResource Crawl { get; }

    /// <summary>Submit and retrieve batch scrape jobs across multiple URLs.</summary>
    public BatchResource Batch { get; }

    /// <summary>Execute turnkey extraction templates for 100+ popular platforms.</summary>
    public TemplatesResource Templates { get; }

    /// <summary>Save and rerun reusable scraping or template configurations.</summary>
    public PresetsResource Presets { get; }

    /// <summary>Create and manage scheduled website change monitors and alerts.</summary>
    public MonitorsResource Monitors { get; }

    /// <summary>Retrieve account plan details, credit balances, and bandwidth usage.</summary>
    public AccountResource Account { get; }

    /// <summary>Retrieve execution audit logs across scrape, search, map, and parse.</summary>
    public LogsResource Logs { get; }

    /// <summary>Retrieve credit and request usage statistics.</summary>
    public UsageResource Usage { get; }

    /// <summary>
    /// Initialise a new Spidra client.
    /// </summary>
    /// <param name="apiKey">Your Spidra API key. Get one at app.spidra.io.</param>
    /// <param name="httpClient">
    /// Optional <see cref="HttpClient"/> to use. Useful for testing or when you need
    /// custom timeout/proxy settings. If omitted, a shared client is created.
    /// </param>
    public SpidraClient(string apiKey, HttpClient? httpClient = null)
        : this(apiKey, null, httpClient)
    {
    }

    /// <summary>
    /// Initialise a new Spidra client with custom base URL.
    /// </summary>
    /// <param name="apiKey">Your Spidra API key.</param>
    /// <param name="baseUrl">Custom API base URL (default: https://api.spidra.io/api).</param>
    /// <param name="httpClient">Optional <see cref="HttpClient"/>.</param>
    public SpidraClient(string apiKey, string? baseUrl, HttpClient? httpClient = null)
    {
        _http = new SpidraHttpClient(apiKey, baseUrl, httpClient);
        Scrape = new ScrapeResource(_http);
        Search = new SearchResource(_http);
        Map = new MapResource(_http);
        Parse = new ParseResource(_http);
        Crawl = new CrawlResource(_http);
        Batch = new BatchResource(_http);
        Templates = new TemplatesResource(_http);
        Presets = new PresetsResource(_http);
        Monitors = new MonitorsResource(_http);
        Account = new AccountResource(_http);
        Logs = new LogsResource(_http);
        Usage = new UsageResource(_http);
    }

    /// <summary>Convenience helper to scrape a URL and wait for completion.</summary>
    public Task<ScrapeJobResponse> ScrapeAsync(
        string url,
        string? prompt = null,
        OutputFormat? output = null,
        TimeSpan? timeout = null,
        CancellationToken cancellationToken = default)
        => Scrape.RunAsync(url, prompt, output, timeout, cancellationToken);

    /// <summary>Convenience helper to search with a natural language query and wait for results.</summary>
    public Task<SearchJobResponse> SearchAsync(
        string query,
        TimeSpan? timeout = null,
        CancellationToken cancellationToken = default)
        => Search.RunQueryAsync(query, timeout, cancellationToken);

    /// <summary>Convenience helper to map a domain starting from a URL.</summary>
    public Task<MapResult> MapAsync(
        string url,
        string? search = null,
        CancellationToken cancellationToken = default)
        => Map.RunUrlAsync(url, search, cancellationToken);

    /// <summary>Convenience helper to parse a document URL and wait for extracted markdown/text.</summary>
    public Task<ParseJobResponse> ParseAsync(
        string url,
        string? mode = null,
        TimeSpan? timeout = null,
        CancellationToken cancellationToken = default)
        => Parse.RunUrlAsync(url, mode, timeout, cancellationToken);

    /// <summary>Convenience helper to crawl a seed URL and wait for collected pages.</summary>
    public Task<CrawlJobResponse> CrawlAsync(
        string url,
        string? crawlInstruction = null,
        string? transformInstruction = null,
        TimeSpan? timeout = null,
        CancellationToken cancellationToken = default)
        => Crawl.RunUrlAsync(url, crawlInstruction, transformInstruction, timeout, cancellationToken);

    /// <summary>Convenience helper to batch scrape a list of URLs and wait for all results.</summary>
    public Task<BatchJobResponse> BatchAsync(
        List<string> urls,
        string? prompt = null,
        OutputFormat? output = null,
        TimeSpan? timeout = null,
        CancellationToken cancellationToken = default)
        => Batch.RunAsync(urls, prompt, output, timeout, cancellationToken);

    /// <summary>Convenience helper to execute a turnkey template and wait for extracted rows.</summary>
    public Task<TemplateRunResponse> RunTemplateAsync(
        string slug,
        Dictionary<string, object?>? inputs = null,
        TimeSpan? timeout = null,
        CancellationToken cancellationToken = default)
        => Templates.RunAsync(slug, inputs, timeout, null, cancellationToken);

    /// <summary>Convenience helper to execute a saved preset.</summary>
    public Task<RunPresetResponse> RunPresetAsync(
        string presetId,
        Dictionary<string, object?>? overrides = null,
        bool wait = true,
        CancellationToken cancellationToken = default)
        => Presets.RunAsync(presetId, overrides, wait, cancellationToken);

    /// <summary>Convenience helper to retrieve account subscription and credit balance.</summary>
    public Task<AccountSummary> GetAccountAsync(CancellationToken cancellationToken = default)
        => Account.GetAsync(cancellationToken);
}
