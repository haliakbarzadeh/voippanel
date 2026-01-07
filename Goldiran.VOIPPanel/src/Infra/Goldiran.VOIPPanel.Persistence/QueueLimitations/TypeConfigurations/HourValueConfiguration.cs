using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.Persistence.LoginHistories.TypeConfigurations;

public class HourValueConfiguration : IEntityTypeConfiguration<HourValue>
{
    public void Configure(EntityTypeBuilder<HourValue> builder)
    {
        builder.ToTable("HourValue");
    }
}