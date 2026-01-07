using Goldiran.VOIPPanel.Domain.AggregatesModel.TempFlatDataJobs;
using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.Persistence.LoginHistories.TypeConfigurations;

public class TempFlatDataJobConfiguration : IEntityTypeConfiguration<TempFlatDataJob>
{
    public void Configure(EntityTypeBuilder<TempFlatDataJob> builder)
    {
        builder.Property(c => c.Message).HasMaxLength(800);

        builder.ToTable("TemppFlatDataJob");
    }
}