using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories;
using Goldiran.VOIPPanel.Domain.AggregatesModel.MonitoredPositions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.Persistence.LoginHistories.TypeConfigurations;

public class MonitoredPositionConfiguration : IEntityTypeConfiguration<MonitoredPosition>
{
    public void Configure(EntityTypeBuilder<MonitoredPosition> builder)
    {
        builder.HasOne<Position>()
            .WithMany()
            .HasForeignKey("SourcePositionId");

        builder.HasOne<Position>()
            .WithMany()
            .HasForeignKey("DestPositionId");

        builder.ToTable("MonitoredPosition");
    }
}