# Spidra .NET SDK

Official .NET SDK for [Spidra](https://spidra.io) — AI-powered web scraping with proxy rotation and CAPTCHA handling.

Spidra lets you extract structured data from any website by describing what you want in plain English. It handles JavaScript rendering, anti-bot bypass, and CAPTCHA solving as a managed API, so your code stays focused on the data.

## Installation

```bash
dotnet add package Spidra
```

Requires .NET 8 or later.

## Authentication

All requests require an API key sent as the `x-api-key` header. Create yours in the [Spidra dashboard](https://app.spidra.io) under **Settings → API Keys**, then pass it to the client:

```csharp
var client = new SpidraClient(Environment.GetEnvironmentVariable("SPIDRA_API_KEY")!);
```

Keep your API key out of source control. Reading it from an environment variable or a secrets manager is the recommended approach.

---

## Quick start

```csharp
using Spidra;
using Spidra.Types.Scrape;

var client = new SpidraClient(Environment.GetEnvironmentVariable("SPIDRA_API_KEY")!);

var job = await client.Scrape.RunAsync(new ScrapeParams
{
    Urls = [new ScrapeUrl("https://news.ycombinator.com")],
    Prompt = "List the top 5 stories with title, points, and comment count",
    UseProxy = true
});

Console.WriteLine(job.Result.Content);
```

`RunAsync` submits the job and polls until it completes, then returns the result.

---

## Structured output

Pass a JSON schema to get a typed, deserializable result instead of raw text:

```csharp
using System.Text.Json;
using Spidra.Types.Scrape;

var job = await client.Scrape.RunAsync(new ScrapeParams
{
    Urls = [new ScrapeUrl("https://jobs.example.com/senior-engineer")],
    Prompt = "Extract the job title, company, location, and required skills",
    Output = OutputFormat.Json,
    UseProxy = true,
    Schema = JsonSerializer.SerializeToElement(new
    {
        type = "object",
        required = new[] { "title", "company" },
        properties = new
        {
            title = new { type = "string" },
            company = new { type = "string" },
            location = new { type = new[] { "string", "null" } },
            skills = new { type = "array", items = new { type = "string" } }
        }
    })
});

var listing = job.Result.Content.Deserialize<JobListing>(new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
});

Console.WriteLine($"{listing!.Title} at {listing.Company}");
Console.WriteLine($"Skills: {string.Join(", ", listing.Skills)}");

record JobListing(string Title, string Company, string? Location, List<string> Skills);
```

Fields in `required` always appear in the response (as `null` if the data is not found). Optional fields are omitted when unavailable.

---

## Batch scraping

Process multiple URLs in one call:

```csharp
using Spidra.Types.Batch;

var batch = await client.Batch.RunAsync(new BatchScrapeParams
{
    Urls =
    [
        "https://competitor-a.com/pricing",
        "https://competitor-b.com/pricing",
        "https://competitor-c.com/pricing"
    ],
    Prompt = "Extract all pricing plans with name and monthly price",
    Output = OutputFormat.Json,
    UseProxy = true
});

var succeeded = batch.Items.Where(i => i.Status == "completed").ToList();
Console.WriteLine($"{succeeded.Count}/{batch.Items.Count} succeeded");

foreach (var item in succeeded)
{
    Console.WriteLine($"{item.Url}: {item.Result}");
}

// Retry any failures
if (batch.Items.Any(i => i.Status == "failed"))
    await client.Batch.RetryAsync(batch.BatchId);
```

---

## Site crawling

Crawl an entire site and extract structured data from each page:

```csharp
using Spidra.Types.Crawl;

var job = await client.Crawl.RunAsync(new CrawlParams
{
    BaseUrl = "https://example.com/blog",
    CrawlInstruction = "Find all blog posts published in 2024",
    TransformInstruction = "Extract title, author, publish date, and summary",
    MaxPages = 30,
    UseProxy = true
});

foreach (var page in job.Result)
{
    Console.WriteLine($"{page.Url}: {page.Data}");
}
```

---

## Submit and poll manually

If you need to track progress yourself, use `SubmitAsync` and `GetAsync` directly:

```csharp
var job = await client.Scrape.SubmitAsync(new ScrapeParams
{
    Urls = [new ScrapeUrl("https://example.com")],
    Prompt = "Extract the main heading"
});

Console.WriteLine($"Job submitted: {job.JobId}");

while (job.Status is not ("completed" or "failed"))
{
    await Task.Delay(TimeSpan.FromSeconds(2));
    job = await client.Scrape.GetAsync(job.JobId);
    Console.WriteLine($"Status: {job.Status}");
}
```

---

## Error handling

All exceptions inherit from `SpidraException`.

| Exception | When |
|---|---|
| `SpidraAuthenticationException` | 401 — invalid or missing API key |
| `SpidraInsufficientCreditsException` | 402 — not enough credits |
| `SpidraRateLimitException` | 429 — rate limit exceeded |
| `SpidraServerException` | 5xx — server-side error |

```csharp
using Spidra.Exceptions;

try
{
    var job = await client.Scrape.RunAsync(scrapeParams);
    return job.Result.Content;
}
catch (SpidraAuthenticationException)
{
    logger.LogError("Invalid API key. Check your SPIDRA_API_KEY.");
    throw;
}
catch (SpidraInsufficientCreditsException)
{
    logger.LogWarning("Out of scraping credits. Upgrade at spidra.io.");
    throw;
}
catch (SpidraRateLimitException ex)
{
    await Task.Delay(ex.RetryAfter ?? TimeSpan.FromSeconds(5));
    // retry...
}
catch (SpidraServerException)
{
    logger.LogError("Spidra server error.");
    throw;
}
```

`SpidraRateLimitException.RetryAfter` contains the server-suggested wait time when available.

---

## Links

- [Documentation](https://docs.spidra.io)
- [API reference](https://docs.spidra.io/api-reference)
- [Dashboard](https://app.spidra.io)
- [spidra.io](https://spidra.io)
