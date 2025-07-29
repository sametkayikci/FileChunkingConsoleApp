namespace Infrastructure.ChunkStrategies;

public sealed class ChunkStrategyFactory(IConfiguration config) : IChunkStrategyFactory
{
    public IChunkStrategy Create(ChunkStrategyType strategy) => strategy switch
    {
        ChunkStrategyType.Dynamic => new DynamicChunkStrategy(config),
        ChunkStrategyType.Fixed => new FixedSizeChunkStrategy(config),
        ChunkStrategyType.TargetCount => new TargetChunkCountStrategy(config),
        _ => throw new ArgumentOutOfRangeException(nameof(strategy), "Unknown strategy type")
    };
}
