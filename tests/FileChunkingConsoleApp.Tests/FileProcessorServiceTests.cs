namespace FileChunkingConsoleApp.Tests;

public class FileProcessorServiceTests
{
    [Fact]
    public async Task ChunkAndUploadAsync_FileNotFound_ShouldThrow()
    {
        var strategy = new Mock<IChunkStrategy>().Object;
        var providers = new List<IStorageProvider>();
        var repo = new Mock<IMetadataRepository>().Object;
        var logger = Mock.Of<ILogger<FileProcessorService>>();
        var service = new FileProcessorService(strategy, providers, repo, logger);

        await Assert.ThrowsAsync<FileNotFoundException>(() =>
            service.ChunkAndUploadAsync("notfound.txt", CancellationToken.None));
    }

    [Fact]
    public async Task MergeChunksAsync_ChecksumMismatch_ShouldThrow()
    {
        var chunk = new ChunkMetadata
        {
            Id = Guid.NewGuid(),
            FileId = Guid.NewGuid(),
            Sequence = 0,
            Size = 4,
            StorageProvider = "Mock",
            Checksum = "WRONG"
        };

        var meta = new FileMetadata(chunk.FileId, "test.txt", 4, "WRONG");

        var mockRepo = new Mock<IMetadataRepository>();
        mockRepo.Setup(r => r.GetFileAsync(chunk.FileId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(meta);
        mockRepo.Setup(r => r.GetChunksAsync(chunk.FileId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([chunk]);

        var mockProvider = new Mock<IStorageProvider>();
        mockProvider.SetupGet(p => p.Name).Returns("Mock");
        mockProvider.Setup(p => p.DownloadChunkAsync(chunk, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new MemoryStream([1, 2, 3, 4]));

        var logger = Mock.Of<ILogger<FileProcessorService>>();

        var service = new FileProcessorService(
            Mock.Of<IChunkStrategy>(),
            new List<IStorageProvider> { mockProvider.Object },
            mockRepo.Object,
            logger
        );

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.MergeChunksAsync(chunk.FileId, "output.dat", CancellationToken.None));
    }
}