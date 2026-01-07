using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories;
using File=Goldiran.VOIPPanel.Domain.AggregatesModel.Files.File;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;

namespace Goldiran.VOIPPanel.Persistence.Files.TypeConfigurations;

public class FileConfiguration : IEntityTypeConfiguration<File>
{
    public void Configure(EntityTypeBuilder<File> builder)
    {
        builder.HasKey(e => e.Id);


        builder.Property(e => e.FileOwnerTypeId)
            .IsRequired();

        builder.Property(t => t.FileName)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(t => t.Name)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(t => t.ContentType)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(t => t.Length)
            .IsRequired();

        builder.HasOne(e => e.FileContent)
            .WithOne()
            .HasForeignKey<FileContent>("FileId").OnDelete(DeleteBehavior.Cascade);
        builder.ToTable("File");
    }
}