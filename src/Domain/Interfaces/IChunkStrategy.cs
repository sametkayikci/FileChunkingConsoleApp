namespace Domain.Interfaces;

public interface IChunkStrategy
{
    IEnumerable<ChunkMetadata> Split(FileInfo file);
}
