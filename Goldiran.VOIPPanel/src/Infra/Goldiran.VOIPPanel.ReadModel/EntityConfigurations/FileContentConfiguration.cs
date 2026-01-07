using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories;
using FileContent=Goldiran.VOIPPanel.ReadModel.Entities.FileContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;

namespace Goldiran.VOIPPanel.QueryHandler.EntityConfigurations;

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