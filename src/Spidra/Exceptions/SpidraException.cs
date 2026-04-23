namespace Spidra.Exceptions;

/// <summary>Base exception for all Spidra API errors.</summary>
public class SpidraException : Exception
{
    public int? StatusCode { get; }

    public SpidraException(string message, int? statusCode = null)
        : base(message)
    {
        StatusCode = statusCode;
    }
}
