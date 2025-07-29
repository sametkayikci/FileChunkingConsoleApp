namespace Infrastructure.ChunkStrategies;

public sealed class TargetChunkCountStrategy(IConfiguration config) : IChunkStrategy
{
    public IEnumerable<ChunkMetadata> Split(FileInfo file)
    {
        var count = config.GetValue<int>("Chunking:TargetCount");
        var size = (long)Math.Ceiling((double)file.Length / count);
        return ChunkGenerator.Generate(file, count, size);
    }
}