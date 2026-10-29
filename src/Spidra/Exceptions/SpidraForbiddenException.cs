namespace Spidra.Exceptions;

/// <summary>Thrown when access to a resource or domain is forbidden (HTTP 403).</summary>
public class SpidraForbiddenException : SpidraException
{
    public SpidraForbiddenException(string message = "Access forbidden.", string? responseBody = null)
        : base(message, 403, responseBody) { }
}

