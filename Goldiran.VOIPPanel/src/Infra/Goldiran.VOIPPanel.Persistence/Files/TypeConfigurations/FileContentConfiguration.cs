using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories;
using File=Goldiran.VOIPPanel.Domain.AggregatesModel.Files.File;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;

namespace Goldiran.VOIPPanel.Persistence.Files.TypeConfigurations;

public class FileContentConfiguration : IEntityTypeConfiguration<FileContent>
{
    public void Configure(EntityTypeBuilder<FileContent> builder)
    {
        //builder.HasOne<File>()
        //    .WithOne()
        //    .HasForeignKey<File>("FileId");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Content)
            .IsRequired();
        builder.ToTable("FileContent");
    }
}