namespace Infrastructure.Db.Configurations;

public sealed class FileMetadataConfiguration : IEntityTypeConfiguration<FileMetadata>
{
    public void Configure(EntityTypeBuilder<FileMetadata> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(f => f.TotalSize)
            .IsRequired();

        builder.Property(f => f.Checksum)
            .IsRequired()
            .HasMaxLength(128);
    }
}