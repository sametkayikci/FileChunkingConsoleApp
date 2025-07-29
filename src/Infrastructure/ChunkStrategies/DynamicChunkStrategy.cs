namespace Infrastructure.ChunkStrategies;

public sealed class DynamicChunkStrategy(IConfiguration config) : IChunkStrategy
{
    public IEnumerable<ChunkMetadata> Split(FileInfo file)
    {
        var maxChunk = config.GetValue<long>("Chunking:MaxChunkSize");
        var count = (int)Math.Ceiling((double)file.Length / maxChunk);
        return ChunkGenerator.Generate(file, count, maxChunk);
    }
}