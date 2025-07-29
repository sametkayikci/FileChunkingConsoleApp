namespace Application.Helpers;

public static class ChecksumHelper
{
    public static string CalculateSha256(byte[] data)
    {
        var hash = SHA256.HashData(data);
        return Convert.ToHexString(hash);
    }
}
