namespace Spidra.Exceptions;

/// <summary>Thrown when payment is required or credits are depleted (HTTP 402).</summary>
public class SpidraPaymentRequiredException : SpidraInsufficientCreditsException
{
    public SpidraPaymentRequiredException(string message = "Payment required. Upgrade your plan at spidra.io.", string? responseBody = null)
        : base(message, responseBody) { }
}

