namespace Domain.Interfaces;

public interface IChunkStrategyFactory
{
    IChunkStrategy Create(ChunkStrategyType strategy);
}
