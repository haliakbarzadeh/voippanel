using Goldiran.VOIPPanel.ReadModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.QueryHandler.EntityConfigurations;

public class FlatDataJobConfiguration : IEntityTypeConfiguration<FlatDataJob>
{
    public void Configure(EntityTypeBuilder<FlatDataJob> builder)
    {
        builder.HasKey(e => e.Id);

        builder.ToTable("FlatDataJob");
    }
}