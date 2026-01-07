using Goldiran.VOIPPanel.Domain.AggregatesModel.FlatDataJobs;
using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.Persistence.LoginHistories.TypeConfigurations;

public class FlatDataJobConfiguration : IEntityTypeConfiguration<FlatDataJob>
{
    public void Configure(EntityTypeBuilder<FlatDataJob> builder)
    {
        builder.Property(c => c.Message).HasMaxLength(800);

        builder.ToTable("FlatDataJob");
    }
}