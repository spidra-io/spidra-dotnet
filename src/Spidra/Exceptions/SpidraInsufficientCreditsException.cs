namespace Spidra.Exceptions;

/// <summary>Thrown when the account has insufficient scraping credits (HTTP 402).</summary>
public class SpidraInsufficientCreditsException : SpidraException
{
    public SpidraInsufficientCreditsException(string message = "Insufficient credits. Upgrade your plan at spidra.io.", string? responseBody = null)
        : base(message, 402, responseBody) { }
}
