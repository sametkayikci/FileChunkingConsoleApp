namespace Infrastructure.Entities;

public sealed class ChunkBlobEntity
{
    public Guid Id { get; init; }
    public byte[] Data { get; init; }
}
