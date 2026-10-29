using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Spidra.Exceptions;
using Spidra.Types.Map;
using Spidra.Types.Parse;
using Spidra.Types.Scrape;
using Spidra.Types.Search;
using Xunit;

namespace Spidra.Tests;

public class SpidraTests
{
    [Fact]
    public void ClientInitialization_InitializesAllResources()
    {
        var client = new SpidraClient("test_api_key");

        Assert.NotNull(client.Scrape);
        Assert.NotNull(client.Search);
        Assert.NotNull(client.Map);
        Assert.NotNull(client.Parse);
        Assert.NotNull(client.Crawl);
        Assert.NotNull(client.Batch);
        Assert.NotNull(client.Templates);
        Assert.NotNull(client.Presets);
        Assert.NotNull(client.Monitors);
        Assert.NotNull(client.Account);
        Assert.NotNull(client.Logs);
        Assert.NotNull(client.Usage);
    }

    [Fact]
    public void WebhookVerify_ValidSignature_ReturnsTrue()
    {
        var payload = "{\"event\":\"monitor.change\",\"monitorId\":\"mon_123\",\"hasChanges\":true}";
        var secret = "whsec_test_secret_key_123456";

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        var hex = Convert.ToHexString(hash).ToLowerInvariant();

        // With sha256= prefix
        Assert.True(Webhook.Verify(payload, $"sha256={hex}", secret));

        // Without prefix
        Assert.True(Webhook.Verify(payload, hex, secret));

        // Byte array overload
        Assert.True(Webhook.Verify(Encoding.UTF8.GetBytes(payload), $"sha256={hex}", secret));
    }

    [Fact]
    public void WebhookVerify_InvalidSignature_ReturnsFalse()
    {
        var payload = "{\"event\":\"monitor.change\"}";
        var secret = "whsec_test_secret_key_123456";

        Assert.False(Webhook.Verify(payload, "sha256=0000000000000000000000000000000000000000000000000000000000000000", secret));
        Assert.False(Webhook.Verify(payload, "invalid_format", secret));
        Assert.False(Webhook.Verify(payload, "sha256=abcdef", "wrong_secret"));
        Assert.False(Webhook.Verify("", "sha256=abcdef", secret));
    }

    [Fact]
    public void ExceptionHierarchy_MatchesStatusAndProperties()
    {
        var auth = new SpidraAuthenticationException();
        Assert.Equal(401, auth.StatusCode);

        var cred = new SpidraInsufficientCreditsException();
        Assert.Equal(402, cred.StatusCode);

        var forb = new SpidraForbiddenException();
        Assert.Equal(403, forb.StatusCode);

        var notFound = new SpidraNotFoundException();
        Assert.Equal(404, notFound.StatusCode);

        var val = new SpidraValidationException("Validation failed", ["Field 'url' is required", "Invalid limit"], 422);
        Assert.Equal(422, val.StatusCode);
        Assert.Equal(2, val.Errors.Count);

        var rate = new SpidraRateLimitException("Rate limit", TimeSpan.FromSeconds(30));
        Assert.Equal(429, rate.StatusCode);
        Assert.Equal(TimeSpan.FromSeconds(30), rate.RetryAfter);

        var server = new SpidraServerException("Server crashed", 503);
        Assert.Equal(503, server.StatusCode);

        var failed = new SpidraJobFailedException("Job failed", "job_123", "failed");
        Assert.Equal("job_123", failed.JobId);
        Assert.Equal("failed", failed.Status);

        var timeout = new SpidraTimeoutException("Polling timed out", "job_456", 120);
        Assert.Equal("job_456", timeout.JobId);
        Assert.Equal(120, timeout.TimeoutSeconds);
    }

    [Fact]
    public void SearchParams_Serialization_ProducesCorrectJson()
    {
        var @params = new SearchParams
        {
            Query = "quantum computing",
            Sources = ["web", "news"],
            Limit = 10,
            Scrape = true,
            ScrapeOptions = new SearchScrapeOptions
            {
                Output = "markdown"
            }
        };

        var json = JsonSerializer.Serialize(@params, Spidra.Http.SpidraHttpClient.JsonOptions);

        Assert.Contains("\"query\":\"quantum computing\"", json);
        Assert.Contains("\"sources\":[\"web\",\"news\"]", json);
        Assert.Contains("\"limit\":10", json);
        Assert.Contains("\"scrape\":true", json);
        Assert.Contains("\"scrapeOptions\":{\"output\":\"markdown\"}", json);
    }

    [Fact]
    public void ScrapeParams_WithBrowserActions_Serialization_ProducesCorrectJson()
    {
        var @params = new ScrapeParams
        {
            Urls = [new ScrapeUrl("https://example.com")],
            Prompt = "Extract table items",
            Actions =
            [
                new BrowserAction
                {
                    Type = "click",
                    Selector = "button#accept"
                },
                new BrowserAction
                {
                    Type = "wait",
                    Milliseconds = 1500
                }
            ]
        };

        var json = JsonSerializer.Serialize(@params, Spidra.Http.SpidraHttpClient.JsonOptions);

        Assert.Contains("\"type\":\"click\"", json);
        Assert.Contains("\"selector\":\"button#accept\"", json);
        Assert.Contains("\"type\":\"wait\"", json);
        Assert.Contains("\"milliseconds\":1500", json);
    }
}

