namespace Infrastructure.StorageProviders;

public sealed class DatabaseStorageProvider(ChunkMetadataContext chunkMetadataContext) : IStorageProvider
{
    public string Name => "Database";

    public async Task UploadChunkAsync(ChunkMetadata chunk, Stream data, CancellationToken cancellationToken)
    {
        using var ms = new MemoryStream();
        await data.CopyToAsync(ms, cancellationToken);
        var blob = new ChunkBlobEntity
        {
            Id = chunk.Id,
            Data = ms.ToArray()
        };
        chunkMetadataContext.Add(blob);
        await chunkMetadataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Stream> DownloadChunkAsync(ChunkMetadata chunk, CancellationToken cancellationToken)
    {
        var blob = await chunkMetadataContext.Set<ChunkBlobEntity>().FindAsync([chunk.Id], cancellationToken);
        return new MemoryStream(blob.Data);
    }
}