namespace Domain.Entities;

public sealed record ChunkMetadata
{
    public Guid Id { get; init; }
    public Guid FileId { get; init; }
    public int Sequence { get; init; }
    public long Size { get; init; }
    public string StorageProvider { get; init; } = string.Empty;
    public string Checksum { get; set; } = string.Empty;
}
