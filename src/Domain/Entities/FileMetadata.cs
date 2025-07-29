namespace Domain.Entities;

public sealed record FileMetadata(Guid Id, string FileName, long TotalSize, string Checksum);