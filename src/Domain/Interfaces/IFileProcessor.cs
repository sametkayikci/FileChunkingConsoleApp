namespace Domain.Interfaces;

public interface IFileProcessor
{
    Task ChunkAndUploadAsync(string filePath, CancellationToken cancellationToken);
    Task MergeChunksAsync(Guid fileId, string outputPath, CancellationToken cancellationToken);
}