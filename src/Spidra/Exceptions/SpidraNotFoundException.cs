namespace Spidra.Exceptions;

/// <summary>Thrown when a requested resource or job is not found (HTTP 404).</summary>
public class SpidraNotFoundException : SpidraException
{
    public SpidraNotFoundException(string message = "Resource not found.", string? responseBody = null)
        : base(message, 404, responseBody) { }
}

