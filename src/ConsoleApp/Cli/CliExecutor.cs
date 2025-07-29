namespace ConsoleApp.Cli;

public class CliExecutor(CliOptions options, IConsole console)
{
    public async Task ExecuteAsync()
    {
        var validator = new CliOptionsValidator();
        var result = await validator.ValidateAsync(options, CancellationToken.None);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                console.Error.WriteLine($"{error.ErrorMessage}");
            return;
        }

        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection()
            .AddLogging(cfg => cfg.AddConsole())
            .AddSingleton<IConfiguration>(config)
            .AddFileChunking(config)
            .BuildServiceProvider();

        var factory = services.GetRequiredService<IChunkStrategyFactory>();
        var chunkStrategy = factory.Create(options.Strategy);

        var processor = new FileProcessorService(
            chunkStrategy,
            services.GetServices<IStorageProvider>(),
            services.GetRequiredService<IMetadataRepository>(),
            services.GetRequiredService<ILogger<FileProcessorService>>()
        );

        var cts = new CancellationTokenSource();

        if (!string.IsNullOrWhiteSpace(options.Process))
            await processor.ChunkAndUploadAsync(options.Process, cts.Token);
        else
            await processor.MergeChunksAsync(options.Merge, options.Output, cts.Token);

    }
}