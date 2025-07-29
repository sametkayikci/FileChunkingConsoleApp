namespace Infrastructure.StorageProviders;

public sealed class FileSystemStorageProvider(IConfiguration config) : IStorageProvider
{
    public string Name => "FileSystem";
    private readonly string _basePath = config.GetValue<string>("Storage:FileSystem:BasePath");

    public async Task UploadChunkAsync(ChunkMetadata chunk, Stream data, CancellationToken cancellationToken)
    {
        var dir = Path.Combine(_basePath, chunk.FileId.ToString());
        Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, chunk.Sequence + ".chunk");
        await using var fs = File.Create(path);
        await data.CopyToAsync(fs, cancellationToken);
    }

    public Task<Stream> DownloadChunkAsync(ChunkMetadata chunk, CancellationToken cancellationToken)
    {
        var path = Path.Combine(_basePath, chunk.FileId.ToString(), chunk.Sequence + ".chunk");
        return Task.FromResult<Stream>(File.OpenRead(path));
    }
}