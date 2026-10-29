namespace Spidra.Exceptions;

/// <summary>Thrown when the Spidra API returns a 5xx server error.</summary>
public class SpidraServerException : SpidraException
{
    public SpidraServerException(string message, int statusCode = 500, string? responseBody = null)
        : base(message, statusCode, responseBody) { }
}
