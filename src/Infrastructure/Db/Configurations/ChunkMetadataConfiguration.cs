namespace Infrastructure.Db.Configurations;

public sealed class ChunkMetadataConfiguration : IEntityTypeConfiguration<ChunkMetadata>
{
    public void Configure(EntityTypeBuilder<ChunkMetadata> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Sequence)
            .IsRequired();

        builder.Property(c => c.Size)
            .IsRequired();

        builder.Property(c => c.StorageProvider)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Checksum)
            .IsRequired()
            .HasMaxLength(128);

        builder.HasOne<FileMetadata>()
            .WithMany()
            .HasForeignKey(c => c.FileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.FileId);
        builder.HasIndex(c => new { c.FileId, c.Sequence }).IsUnique();
    }
}