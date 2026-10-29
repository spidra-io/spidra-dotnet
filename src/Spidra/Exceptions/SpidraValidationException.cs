namespace Spidra.Exceptions;

/// <summary>Thrown when parameters fail validation (HTTP 422 or 400).</summary>
public class SpidraValidationException : SpidraException
{
    /// <summary>Detailed validation errors reported by the API.</summary>
    public IReadOnlyList<string> Errors { get; }

    public SpidraValidationException(
        string message = "Validation failed.",
        IReadOnlyList<string>? errors = null,
        int statusCode = 422,
        string? responseBody = null)
        : base(message, statusCode, responseBody)
    {
        Errors = errors ?? [];
    }
}

