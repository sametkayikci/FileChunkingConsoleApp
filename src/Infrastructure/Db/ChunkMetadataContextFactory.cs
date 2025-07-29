namespace Infrastructure.Db;

public class ChunkMetadataContextFactory : IDesignTimeDbContextFactory<ChunkMetadataContext>
{
    public ChunkMetadataContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = config.GetConnectionString("Postgres");

        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseNpgsql(connectionString);

        return new ChunkMetadataContext(optionsBuilder.Options);
    }
}
