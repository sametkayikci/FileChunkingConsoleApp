using Domain.Entities;

namespace Application.Interfaces;

public interface IMetadataRepository
{
    Task SaveFileAsync(FileMetadata file, CancellationToken cancellationToken);
    Task SaveChunkAsync(ChunkMetadata chunk, CancellationToken cancellationToken);
    Task<FileMetadata> GetFileAsync(Guid fileId, CancellationToken cancellationToken);
    Task<IEnumerable<ChunkMetadata>> GetChunksAsync(Guid fileId, CancellationToken cancellationToken);
}