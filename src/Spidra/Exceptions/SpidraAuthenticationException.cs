namespace Spidra.Exceptions;

/// <summary>Thrown when the API key is missing or invalid (HTTP 401).</summary>
public class SpidraAuthenticationException : SpidraException
{
    public SpidraAuthenticationException(string message = "Invalid or missing API key.")
        : base(message, 401) { }
}
