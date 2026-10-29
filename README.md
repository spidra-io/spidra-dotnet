# Spidra .NET SDK

The official .NET SDK for [Spidra](https://spidra.io) — AI-native web scraping, multi-engine search, file & document parsing, deep site crawling, turnkey extraction templates, saved presets, and scheduled change monitors.

[![NuGet](https://img.shields.io/nuget/v/Spidra.svg?style=flat-square)](https://www.nuget.org/packages/Spidra/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg?style=flat-square)](LICENSE)
[![.NET Version](https://img.shields.io/badge/.NET-%3E%3D8.0-512BD4.svg?style=flat-square)](https://dotnet.microsoft.com/)

---

## Features

- **Web Scraping**: Fast raw content extraction (Markdown, HTML, text, screenshots) or structured AI extraction with guaranteed JSON schemas.
- **Browser Actions**: Automated headless browser interactions: click, type, wait for selectors, infinite scroll, JavaScript evaluation, element screenshots, and multi-step workflows.
- **Multi-Engine Search**: Web search across Google, Reddit, YouTube, TikTok, Hacker News, research papers, and developer issues with optional automatic markdown page scraping.
- **Domain Mapping**: Fast URL discovery via sitemaps and site graphs with subdomain and robots.txt compliance.
- **Document & Media Parsing**: Parse PDFs, Office documents (DOCX, XLSX), audio, and video files with AI or fast layout modes.
- **Deep Site Crawling**: Recursive crawling with natural language instructions, link depth limits, parallel workers, crawl diagnostic reports, and per-page retries.
- **Batch Scraping**: Concurrent scraping of hundreds or thousands of URLs with automatic chunking and cookie-based retry support.
- **Turnkey Templates**: 100+ ready-made extraction workflows for platforms like Google Maps, Amazon, flights, LinkedIn, and real estate.
- **Saved Presets**: Save and rerun common tool and template configurations with a single identifier.
- **Scheduled Monitors**: Watch pages for changes, evaluate CSS or AI conditions, and send webhook/email/Slack notifications.
- **Webhook Verification**: Secure constant-time HMAC-SHA256 signature verification helper.
- **Account & Logs**: Check credit balances, inspect proxy usage, and review detailed execution logs.

---

## Requirements

- .NET 8.0 or later
- C# 12 or later
- Standard library dependencies only (`System.Net.Http`, `System.Text.Json`, `System.Security.Cryptography`)

---

## Installation

```bash
dotnet add package Spidra
```

Get your API key at [app.spidra.io](https://app.spidra.io) under **Settings → API Keys**.

---

## Quick Start

```csharp
using Spidra;
using Spidra.Types.Scrape;

var client = new SpidraClient(Environment.GetEnvironmentVariable("SPIDRA_API_KEY")!);

// Scrape structured data with AI
var job = await client.ScrapeAsync(
    url: "https://news.ycombinator.com",
    prompt: "Extract the top 5 articles with title, points, author, and URL",
    output: OutputFormat.Json
);

Console.WriteLine(job.Result?.Content);
```

---

## Table of Contents

- [Client Initialization](#client-initialization)
- [Scraping](#scraping)
  - [Simple scrape](#simple-scrape)
  - [Structured AI extraction with JSON schema](#structured-ai-extraction-with-json-schema)
  - [Browser actions & interactions](#browser-actions--interactions)
  - [Geo-targeted scraping](#geo-targeted-scraping)
  - [Raw formats (Markdown, Screenshots)](#raw-formats-markdown-screenshots)
- [Search](#search)
  - [Basic web search](#basic-web-search)
  - [Search with automatic page scraping](#search-with-automatic-page-scraping)
  - [Specialized sources and domain filters](#specialized-sources-and-domain-filters)
- [Map](#map)
- [Document Parsing](#document-parsing)
- [Crawling](#crawling)
  - [Start and wait for a crawl](#start-and-wait-for-a-crawl)
  - [Crawl reports and page retries](#crawl-reports-and-page-retries)
- [Batch Scraping](#batch-scraping)
- [Ready-Made Templates](#ready-made-templates)
- [Saved Presets](#saved-presets)
- [Scheduled Monitors](#scheduled-monitors)
- [Account & Usage](#account--usage)
- [Execution Logs](#execution-logs)
- [Webhook Verification](#webhook-verification)
- [Error Handling](#error-handling)

---

## Client Initialization

```csharp
using Spidra;

// Default initialization
var client = new SpidraClient(Environment.GetEnvironmentVariable("SPIDRA_API_KEY")!);

// With custom base URL or custom HttpClient
var customClient = new SpidraClient(
    apiKey: "your-api-key",
    baseUrl: "https://api.spidra.io/api",
    httpClient: new HttpClient { Timeout = TimeSpan.FromSeconds(60) }
);
```

---

## Scraping

### Simple scrape

Pass a target URL directly:

```csharp
var job = await client.ScrapeAsync("https://example.com");
Console.WriteLine(job.Result?.Content);
```

### Structured AI extraction with JSON schema

When you need guaranteed output structure, provide a JSON Schema object:

```csharp
using Spidra.Types.Scrape;

var schema = new
{
    type = "object",
    required = new[] { "company", "valuation", "headquarters" },
    properties = new
    {
        company = new { type = "string" },
        valuation = new { type = "string" },
        headquarters = new { type = "string" }
    }
};

var job = await client.Scrape.RunAsync(new ScrapeParams
{
    Urls = [new ScrapeUrl("https://example.com/company")],
    Prompt = "Extract company profile and valuation details",
    Output = OutputFormat.Json,
    Schema = schema
});

Console.WriteLine(job.Result?.Content);
```

### Browser actions & interactions

Automate clicks, keystrokes, scrolling, and waits before scraping:

```csharp
var job = await client.Scrape.RunAsync(new ScrapeParams
{
    Urls =
    [
        new ScrapeUrl("https://example.com/protected",
        [
            new BrowserAction { Type = "click", Selector = "button#accept-cookies" },
            new BrowserAction { Type = "wait", Milliseconds = 1000 },
            new BrowserAction { Type = "scroll", Text = "down" }
        ])
    ],
    Prompt = "Extract product items"
});
```

### Geo-targeted scraping

Route requests through residential proxies in specific countries:

```csharp
var job = await client.Scrape.RunAsync(new ScrapeParams
{
    Urls = [new ScrapeUrl("https://example.com/regional-pricing")],
    UseProxy = true,
    ProxyCountry = "US" // US, GB, DE, FR, JP, etc.
});
```

### Raw formats (Markdown, Screenshots)

```csharp
var job = await client.Scrape.RunAsync(new ScrapeParams
{
    Urls = [new ScrapeUrl("https://example.com")],
    Output = OutputFormat.Markdown,
    FullPageScreenshot = true
});

Console.WriteLine("Markdown: " + job.Result?.Content);
if (job.Result?.Screenshots.Count > 0)
{
    Console.WriteLine("Screenshot: " + job.Result.Screenshots[0]);
}
```

---

## Search

Query search engines with natural language, recency filters, and optional markdown scraping:

### Basic web search

```csharp
var search = await client.SearchAsync("latest advances in quantum computing");

foreach (var item in search.Result?.Results ?? [])
{
    Console.WriteLine($"{item.Title} - {item.Url}");
}
```

### Search with automatic page scraping

Fetch search results and automatically scrape each target page into markdown:

```csharp
using Spidra.Types.Search;

var search = await client.Search.RunAsync(new SearchParams
{
    Query = "best noise cancelling headphones 2026",
    Sources = ["web"],
    Limit = 5,
    Scrape = true,
    ScrapeOptions = new SearchScrapeOptions
    {
        Output = "markdown"
    }
});

foreach (var item in search.Result?.Results ?? [])
{
    Console.WriteLine("URL: " + item.Url);
    Console.WriteLine("Scraped Content: " + item.ScrapedContent);
}
```

### Specialized sources and domain filters

```csharp
var search = await client.Search.RunAsync(new SearchParams
{
    Query = "C# async await best practices",
    Sources = ["developer", "web"],
    Domains = ["github.com", "microsoft.com"],
    Recency = "past_year",
    Limit = 10
});
```

---

## Map

Discover all reachable URLs on a website using site graphs and sitemaps:

```csharp
var map = await client.MapAsync("https://learn.microsoft.com/dotnet");

Console.WriteLine($"Discovered {map.TotalLinks} links:");
foreach (var link in map.Links)
{
    Console.WriteLine(link.Url);
}

// Or filter links matching a keyword
var filtered = await client.MapAsync("https://learn.microsoft.com/dotnet", search: "csharp");
```

---

## Document Parsing

Parse PDFs, DOCX, XLSX, audio, and video files with layout extraction:

```csharp
var parsed = await client.ParseAsync("https://example.com/annual_report.pdf");

Console.WriteLine("Markdown content:\n" + parsed.Result?.Markdown);
foreach (var page in parsed.Pages)
{
    Console.WriteLine($"Page {page.PageNumber}: {page.Markdown?.Length} characters");
}
```

### Estimate parsing costs

```csharp
var estimate = await client.Parse.EstimateUrlAsync("https://example.com/large_document.pdf");
Console.WriteLine($"Pages: {estimate.PageCount}, Estimated Credits: {estimate.EstimatedCredits:F2}");
```

---

## Crawling

### Start and wait for a crawl

Recursively crawl an entire website with natural language guidance:

```csharp
using Spidra.Types.Crawl;

var crawl = await client.Crawl.RunAsync(new CrawlParams
{
    BaseUrl = "https://devblogs.microsoft.com/dotnet",
    CrawlInstruction = "Only follow .NET 9 and C# 13 announcement posts",
    TransformInstruction = "Extract the post title, author, date, and key features",
    MaxPages = 20
});

foreach (var page in crawl.Result)
{
    Console.WriteLine($"{page.Title}: {page.Url}");
    Console.WriteLine("Data: " + page.Data);
}
```

### Crawl reports and page retries

Inspect skipped URLs or retry specific failed pages:

```csharp
// Fetch crawl diagnostic report
var report = await client.Crawl.ReportAsync(crawl.JobId);
Console.WriteLine($"Skipped URLs: {report.Report?.SkippedUrls.Count}");

// Retry a specific failed page
var retry = await client.Crawl.RetryPageAsync(crawl.JobId, "page_123");

// Cancel an active crawl
var cancel = await client.Crawl.CancelAsync(crawl.JobId);
```

---

## Batch Scraping

Scrape multiple URLs concurrently with automatic rate limiting:

```csharp
var urls = new List<string>
{
    "https://example.com/product/1",
    "https://example.com/product/2",
    "https://example.com/product/3"
};

var batch = await client.BatchAsync(urls, prompt: "Extract product title and price");

foreach (var item in batch.Items)
{
    Console.WriteLine($"[{item.Status}] {item.Url}: {item.Result}");
}

// Retry failed batch items with session cookies
await client.Batch.RetryAsync(batch.BatchId, cookies: "session_id=abcdef");
```

---

## Ready-Made Templates

Execute turnkey scrapers for 100+ popular platforms:

```csharp
// List available templates
var templates = await client.Templates.ListAsync(new Dictionary<string, string?> { ["category"] = "e-commerce" });
foreach (var t in templates.Templates)
{
    Console.WriteLine($"{t.Slug} - {t.Name}");
}

// Estimate template cost
var est = await client.Templates.EstimateAsync("amazon-product-reviews", new Dictionary<string, object?>
{
    ["url"] = "https://amazon.com/dp/B08N5WRWNW"
});
Console.WriteLine($"Estimated credits: {est.EstimatedCredits:F2}");

// Run template and wait for extracted rows
var run = await client.RunTemplateAsync("amazon-product-reviews", new Dictionary<string, object?>
{
    ["url"] = "https://amazon.com/dp/B08N5WRWNW",
    ["maxItems"] = 50
});

foreach (var row in run.Result)
{
    Console.WriteLine($"Review: {row["title"]} - Rating: {row["rating"]}");
}
```

---

## Saved Presets

Save commonly used scraping or template configurations and re-execute them by ID:

```csharp
using Spidra.Types.Presets;

// Create a preset
var preset = await client.Presets.CreateAsync(new PresetParams
{
    Name = "Daily News Scraper",
    Tool = "scrape",
    Config = new Dictionary<string, object?>
    {
        ["urls"] = new[] { new { url = "https://news.ycombinator.com" } },
        ["prompt"] = "Extract top stories",
        ["output"] = "json"
    }
});

// Run the preset
var res = await client.RunPresetAsync(preset.Id);
Console.WriteLine("Result: " + res.Result);

// Delete preset
await client.Presets.DeleteAsync(preset.Id);
```

---

## Scheduled Monitors

Monitor websites for changes on a recurring schedule with notifications:

```csharp
using Spidra.Types.Monitors;

// Create a monitor
var monitor = await client.Monitors.CreateAsync(new MonitorParams
{
    Name = "Competitor Pricing Monitor",
    Url = "https://example.com/pricing",
    Schedule = "0 * * * *", // hourly cron
    Selector = ".price-table",
    Condition = "Notify if any plan price changes",
    Channels = new Dictionary<string, object?>
    {
        ["webhook"] = "https://myapi.com/webhooks/pricing",
        ["email"] = "alerts@mycompany.com"
    }
});

// Trigger an immediate check
var check = await client.Monitors.CheckNowAsync(monitor.Id);

// Pause and resume
await client.Monitors.PauseAsync(monitor.Id);
await client.Monitors.ResumeAsync(monitor.Id);

// Inspect historical check diffs
var checks = await client.Monitors.ListChecksAsync(monitor.Id);
foreach (var c in checks.Checks)
{
    Console.WriteLine($"Check at {c.CheckedAt}: hasChanges={c.HasChanges}");
}
```

---

## Account & Usage

```csharp
// Account overview: plan, available credits, proxy bandwidth
var account = await client.GetAccountAsync();
Console.WriteLine($"Plan: {account.Plan?.Name} ({account.Plan?.Status})");
Console.WriteLine($"Credits: {account.Credits?.Available:F2} / {account.Credits?.Total:F2}");
Console.WriteLine($"Residential Proxy: {account.ProxyBandwidth?.AvailableMb:F2} MB");

// Usage statistics over time ("7d", "30d", "weekly")
var usage = await client.Account.UsageAsync("30d");
foreach (var row in usage)
{
    Console.WriteLine($"{row.Date}: {row.Requests} requests, {row.Credits:F2} credits");
}

// Tool credit breakdown
var breakdown = await client.Account.BreakdownAsync("7d");
foreach (var b in breakdown)
{
    Console.WriteLine($"[{b.Date}] {b.Tool}: {b.Credits:F2} credits ({b.Requests} requests)");
}
```

---

## Execution Logs

Retrieve audit and debug logs for all operations:

```csharp
// Scrape logs
var scrapeLogs = await client.Logs.ScrapeAsync();

// Search logs
var searchLogs = await client.Logs.SearchAsync();

// Map logs
var mapLogs = await client.Logs.MapAsync();

// Parse logs
var parseLogs = await client.Logs.ParseAsync();
```

---

## Webhook Verification

Verify incoming webhook requests from Spidra using constant-time HMAC-SHA256:

```csharp
using Microsoft.AspNetCore.Mvc;
using Spidra;

[ApiController]
[Route("api/webhook")]
public class WebhookController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> HandleWebhook()
    {
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();

        var signature = Request.Headers["X-Spidra-Signature"].ToString();
        var secret = "whsec_YOUR_WEBHOOK_SECRET";

        if (!Webhook.Verify(body, signature, secret))
        {
            return Unauthorized("Invalid webhook signature");
        }

        // Signature is valid — process event
        return Ok();
    }
}
```

---

## Error Handling

All SDK errors map to specific typed exceptions deriving from `SpidraException`:

```csharp
using Spidra.Exceptions;

try
{
    var job = await client.ScrapeAsync("https://example.com");
}
catch (SpidraAuthenticationException ex)
{
    Console.WriteLine($"Authentication error: {ex.Message}");
}
catch (SpidraInsufficientCreditsException ex)
{
    Console.WriteLine($"Insufficient credits: {ex.Message}");
}
catch (SpidraValidationException ex)
{
    Console.WriteLine($"Validation failed: {ex.Message}");
    foreach (var err in ex.Errors)
    {
        Console.WriteLine($" - {err}");
    }
}
catch (SpidraRateLimitException ex)
{
    Console.WriteLine($"Rate limited. Retry after {ex.RetryAfter?.TotalSeconds}s");
}
catch (SpidraJobFailedException ex)
{
    Console.WriteLine($"Job {ex.JobId} failed with status {ex.Status}: {ex.Message}");
}
catch (SpidraTimeoutException ex)
{
    Console.WriteLine($"Job {ex.JobId} timed out after {ex.TimeoutSeconds}s");
}
catch (SpidraServerException ex)
{
    Console.WriteLine($"Spidra server error {ex.StatusCode}: {ex.Message}");
}
catch (SpidraException ex)
{
    Console.WriteLine($"API error {ex.StatusCode}: {ex.Message}");
}
```

---

## License

Distributed under the [MIT License](LICENSE).
