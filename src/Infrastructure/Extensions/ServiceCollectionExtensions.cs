using Infrastructure.StorageProviders;

namespace Infrastructure.Extensions;


public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFileChunking(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ChunkMetadataContext>(opt =>
            opt.UseNpgsql(config.GetConnectionString("Postgres")));

        services.AddScoped<IMetadataRepository, MetadataRepository>();
        services.AddScoped<IFileProcessor, FileProcessorService>();
        services.AddSingleton<IChunkStrategyFactory, ChunkStrategyFactory>();
        services.AddTransient<IStorageProvider, FileSystemStorageProvider>();
        services.AddTransient<IStorageProvider, DatabaseStorageProvider>();

        return services;
    }
}
