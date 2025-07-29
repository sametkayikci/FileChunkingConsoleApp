namespace Infrastructure.Extensions;

public static class FileInfoExtensions
{
    public static Guid NameHash(this FileInfo file)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(file.Name.ToLowerInvariant()));
        var guidBytes = new byte[16];
        Array.Copy(hash, guidBytes, 16);
        return new Guid(guidBytes);
    }
}