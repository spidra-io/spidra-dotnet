# Spidra .NET SDK

Official .NET SDK for [Spidra](https://spidra.io) — AI-powered web scraping with proxy rotation and CAPTCHA handling.

## Installation

```bash
dotnet add package Spidra
```

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

## Structured output

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

record JobListing(string Title, string Company, string? Location, List<string> Skills);
```

## Batch scraping

```csharp
using Spidra.Types.Batch;

var batch = await client.Batch.RunAsync(new BatchScrapeParams
{
    Urls =
    [
        "https://competitor-a.com/pricing",
        "https://competitor-b.com/pricing"
    ],
    Prompt = "Extract all pricing plans with name and monthly price",
    Output = OutputFormat.Json,
    UseProxy = true
});

foreach (var item in batch.Items.Where(i => i.Status == "completed"))
{
    Console.WriteLine($"{item.Url}: {item.Result}");
}
```

## Site crawling

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

## Error handling

```csharp
using Spidra.Exceptions;

try
{
    var job = await client.Scrape.RunAsync(params);
}
catch (SpidraAuthenticationException)
{
    // Invalid API key
}
catch (SpidraInsufficientCreditsException)
{
    // Out of credits — upgrade at spidra.io
}
catch (SpidraRateLimitException ex)
{
    // Rate limited — ex.RetryAfter has the suggested wait time
    await Task.Delay(ex.RetryAfter ?? TimeSpan.FromSeconds(5));
}
catch (SpidraServerException)
{
    // 5xx from the API
}
```

## Links

- [Documentation](https://docs.spidra.io)
- [API reference](https://docs.spidra.io/api-reference)
- [spidra.io](https://spidra.io)
