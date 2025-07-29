namespace Infrastructure.Services;

public static class ChunkGenerator
{
    public static IEnumerable<ChunkMetadata> Generate(FileInfo file, int count, long fixedSize)
    {
        for (var i = 0; i < count; i++)
        {
            yield return new ChunkMetadata
            {
                Id = Guid.NewGuid(),
                FileId = file.NameHash(),
                Sequence = i,
                Size = Math.Min(fixedSize, file.Length - i * fixedSize),
                StorageProvider = "FileSystem",
                Checksum = string.Empty
            };
        }
    }
}
