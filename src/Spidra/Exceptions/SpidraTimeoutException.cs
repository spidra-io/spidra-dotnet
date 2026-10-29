namespace Spidra.Exceptions;

/// <summary>Thrown when polling an asynchronous job exceeds the specified timeout.</summary>
public class SpidraTimeoutException : SpidraException
{
    /// <summary>The identifier of the job that timed out.</summary>
    public string JobId { get; }

    /// <summary>The timeout in seconds.</summary>
    public int TimeoutSeconds { get; }

    public SpidraTimeoutException(string message, string jobId, int timeoutSeconds)
        : base(message)
    {
        JobId = jobId;
        TimeoutSeconds = timeoutSeconds;
    }
}

