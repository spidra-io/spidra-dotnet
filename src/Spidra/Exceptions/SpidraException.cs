namespace Spidra.Exceptions;

/// <summary>Base exception for all Spidra API errors.</summary>
public class SpidraException : Exception
{
    /// <summary>The HTTP status code returned by the API, if applicable.</summary>
    public int? StatusCode { get; }

    /// <summary>The raw response body returned by the API, if available.</summary>
    public string? ResponseBody { get; }

    public SpidraException(string message, int? statusCode = null, string? responseBody = null, Exception? innerException = null)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
    }
}
