namespace Spidra.Exceptions;

/// <summary>Thrown when the API rate limit is exceeded (HTTP 429).</summary>
public class SpidraRateLimitException : SpidraException
{
    /// <summary>How long to wait before retrying, if provided by the server.</summary>
    public TimeSpan? RetryAfter { get; }

    public SpidraRateLimitException(string message = "Rate limit exceeded.", TimeSpan? retryAfter = null)
        : base(message, 429)
    {
        RetryAfter = retryAfter;
    }
}
