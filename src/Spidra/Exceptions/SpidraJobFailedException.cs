namespace Spidra.Exceptions;

/// <summary>Thrown when an asynchronous job fails or is cancelled.</summary>
public class SpidraJobFailedException : SpidraException
{
    /// <summary>The identifier of the failed job.</summary>
    public string JobId { get; }

    /// <summary>The final status of the job (e.g. "failed", "cancelled", "error").</summary>
    public string Status { get; }

    public SpidraJobFailedException(string message, string jobId, string status)
        : base(message)
    {
        JobId = jobId;
        Status = status;
    }
}

