namespace Infrastructure.ChunkStrategies;

public sealed class FixedSizeChunkStrategy(IConfiguration config) : IChunkStrategy
{
    public IEnumerable<ChunkMetadata> Split(FileInfo file)
    {
        var fixedSize = config.GetValue<long>("Chunking:FixedSize");
        var count = (int)Math.Ceiling((double)file.Length / fixedSize);
        return ChunkGenerator.Generate(file, count, fixedSize);
    }
}