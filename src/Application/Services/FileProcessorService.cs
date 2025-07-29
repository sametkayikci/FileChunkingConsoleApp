namespace Application.Services;

public class FileProcessorService(
    IChunkStrategy strategy,
    IEnumerable<IStorageProvider> providers,
    IMetadataRepository metaRepo,
    ILogger<FileProcessorService> logger) : IFileProcessor
{
    public async Task ChunkAndUploadAsync(string path, CancellationToken cancellationToken)
    {
        logger.LogInformation("Dosya parçalama ve yükleme işlemi başlatıldı: {FilePath}", path);

        var file = new FileInfo(path);
        if (!file.Exists)
        {
            logger.LogError("Belirtilen dosya bulunamadı: {FilePath}", path);
            throw new FileNotFoundException("Dosya bulunamadı", path);
        }

        var chunks = strategy.Split(file).ToList();
        logger.LogInformation("{ChunkCount} adet parçaya ayrıldı. Kullanılan strateji: {Strategy}",
            chunks.Count, strategy.GetType().Name);

        foreach (var chunk in chunks)
        {
            logger.LogDebug("Parça işleniyor: Sıra {Sequence}, Boyut: {Size} byte", chunk.Sequence, chunk.Size);

            await using var stream = file.OpenRead();
            stream.Seek(chunk.Sequence * chunk.Size, SeekOrigin.Begin);

            var buffer = new byte[chunk.Size];
            await stream.ReadExactlyAsync(buffer, cancellationToken);

            chunk.Checksum = ChecksumHelper.CalculateSha256(buffer);
            logger.LogDebug("Checksum hesaplandı. Parça {Sequence}: {Checksum}", chunk.Sequence, chunk.Checksum);

            await metaRepo.SaveChunkAsync(chunk, cancellationToken);
            logger.LogDebug("Parça metadatası kaydedildi: Sıra {Sequence}", chunk.Sequence);

            var provider = providers.First(p => p.Name == chunk.StorageProvider);
            await provider.UploadChunkAsync(chunk, new MemoryStream(buffer), cancellationToken);
            logger.LogInformation("Parça {Sequence} yüklendi. Sağlayıcı: {Provider}", chunk.Sequence, provider.Name);
        }

        logger.LogInformation("Parçalama ve yükleme işlemi tamamlandı: {FilePath}", path);
    }

    public async Task MergeChunksAsync(Guid fileId, string outputPath, CancellationToken cancellationToken)
    {
        logger.LogInformation("Parçaları birleştirme işlemi başlatıldı. FileId: {FileId}", fileId);

        var meta = await metaRepo.GetFileAsync(fileId, cancellationToken);
        logger.LogDebug("Dosya metadatası getirildi. Beklenen Checksum: {Checksum}", meta.Checksum);

        var chunks = (await metaRepo.GetChunksAsync(fileId, cancellationToken)).OrderBy(c => c.Sequence).ToList();
        logger.LogInformation("{ChunkCount} adet parça bulundu. Birleştirme başlatılıyor.", chunks.Count);

        await using var output = File.Create(outputPath);

        foreach (var chunk in chunks)
        {
            logger.LogDebug("Parça birleştiriliyor: Sıra {Sequence}, Sağlayıcı: {Provider}", chunk.Sequence, chunk.StorageProvider);

            var provider = providers.First(p => p.Name == chunk.StorageProvider);
            await using var data = await provider.DownloadChunkAsync(chunk, cancellationToken);

            var buffer = new byte[chunk.Size];
            await data.ReadExactlyAsync(buffer, cancellationToken);

            var actualChecksum = ChecksumHelper.CalculateSha256(buffer);
            if (actualChecksum != chunk.Checksum)
            {
                logger.LogError("Checksum uyuşmazlığı! Parça {Sequence}, Beklenen: {Expected}, Gerçek: {Actual}",
                    chunk.Sequence, chunk.Checksum, actualChecksum);
                throw new InvalidOperationException("Checksum uyuşmazlığı");
            }

            await output.WriteAsync(buffer, cancellationToken);
            logger.LogInformation("Parça {Sequence} çıktı dosyasına yazıldı", chunk.Sequence);
        }

        var finalChecksum = ChecksumHelper.CalculateSha256(await File.ReadAllBytesAsync(outputPath, cancellationToken));
        if (finalChecksum != meta.Checksum)
        {
            logger.LogError("Son checksum uyuşmazlığı! Beklenen: {Expected}, Gerçek: {Actual}", meta.Checksum, finalChecksum);
            throw new InvalidOperationException("Final checksum uyuşmazlığı");
        }

        logger.LogInformation("Dosya başarıyla birleştirildi. FileId: {FileId}, Çıktı: {OutputPath}", fileId, outputPath);
    }
}
