namespace Domain.Interfaces;

public interface IStorageProvider
{
    string Name { get; }
    Task UploadChunkAsync(ChunkMetadata chunk, Stream data, CancellationToken cancellationToken);
    Task<Stream> DownloadChunkAsync(ChunkMetadata chunk, CancellationToken cancellationToken);
}
