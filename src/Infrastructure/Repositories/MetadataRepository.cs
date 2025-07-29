namespace Infrastructure.Repositories;

public sealed class MetadataRepository(ChunkMetadataContext ctx) : IMetadataRepository
{
    public async Task SaveFileAsync(FileMetadata file, CancellationToken cancellationToken)
    {
        ctx.Files.Add(file);
        await ctx.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChunkAsync(ChunkMetadata chunk, CancellationToken cancellationToken)
    {
        ctx.Chunks.Add(chunk);
        await ctx.SaveChangesAsync(cancellationToken);
    }

    public Task<FileMetadata> GetFileAsync(Guid fileId, CancellationToken cancellationToken) =>
        ctx.Files.FindAsync([fileId], cancellationToken).AsTask();

    public Task<IEnumerable<ChunkMetadata>> GetChunksAsync(Guid fileId, CancellationToken cancellationToken) =>
        Task.FromResult(ctx.Chunks.Where(c => c.FileId == fileId).AsEnumerable());
}