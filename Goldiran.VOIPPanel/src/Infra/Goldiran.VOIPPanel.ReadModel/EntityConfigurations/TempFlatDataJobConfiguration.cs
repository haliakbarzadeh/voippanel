using Goldiran.VOIPPanel.ReadModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.QueryHandler.EntityConfigurations;

public class TempFlatDataJobConfiguration : IEntityTypeConfiguration<TempFlatDataJob>
{
    public void Configure(EntityTypeBuilder<TempFlatDataJob> builder)
    {
        builder.HasKey(e => e.Id);

        builder.ToTable("TemppFlatDataJob");
    }
}