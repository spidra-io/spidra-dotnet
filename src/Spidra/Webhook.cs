using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Spidra;

/// <summary>
/// Utility for verifying Spidra webhook signatures.
/// </summary>
public static class Webhook
{
    private static readonly Regex SignatureRegex = new(@"^sha256=([0-9a-fA-F]{64})$", RegexOptions.Compiled);

    /// <summary>
    /// Verifies the authenticity of a Spidra webhook payload using constant-time comparison.
    /// </summary>
    /// <param name="payload">The raw HTTP request body string.</param>
    /// <param name="signatureHeader">The value of the <c>X-Spidra-Signature</c> header.</param>
    /// <param name="secret">Your webhook signing secret.</param>
    /// <returns><c>true</c> if valid; otherwise <c>false</c>.</returns>
    public static bool Verify(string payload, string signatureHeader, string secret)
    {
        if (payload is null) return false;
        return Verify(Encoding.UTF8.GetBytes(payload), signatureHeader, secret);
    }

    /// <summary>
    /// Verifies the authenticity of a Spidra webhook payload using constant-time comparison.
    /// </summary>
    /// <param name="payloadBytes">The raw HTTP request body bytes.</param>
    /// <param name="signatureHeader">The value of the <c>X-Spidra-Signature</c> header.</param>
    /// <param name="secret">Your webhook signing secret.</param>
    /// <returns><c>true</c> if valid; otherwise <c>false</c>.</returns>
    public static bool Verify(byte[] payloadBytes, string signatureHeader, string secret)
    {
        if (payloadBytes is null || string.IsNullOrWhiteSpace(signatureHeader) || string.IsNullOrWhiteSpace(secret))
            return false;

        var trimmed = signatureHeader.Trim();
        var match = SignatureRegex.Match(trimmed);
        string expectedHex;
        if (match.Success)
        {
            expectedHex = match.Groups[1].Value.ToLowerInvariant();
        }
        else if (trimmed.Length == 64 && Regex.IsMatch(trimmed, @"^[0-9a-fA-F]{64}$"))
        {
            expectedHex = trimmed.ToLowerInvariant();
        }
        else
        {
            return false;
        }

        try
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hashBytes = hmac.ComputeHash(payloadBytes);
            var computedHex = Convert.ToHexString(hashBytes).ToLowerInvariant();

            var expectedBytes = Encoding.UTF8.GetBytes(expectedHex);
            var actualBytes = Encoding.UTF8.GetBytes(computedHex);

            return CryptographicOperations.FixedTimeEquals(expectedBytes, actualBytes);
        }
        catch
        {
            return false;
        }
    }
}

