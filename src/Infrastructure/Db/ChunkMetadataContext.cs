namespace Infrastructure.Db;

public sealed class ChunkMetadataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<FileMetadata> Files { get; set; }
    public DbSet<ChunkMetadata> Chunks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChunkMetadataContext).Assembly);
    }
}