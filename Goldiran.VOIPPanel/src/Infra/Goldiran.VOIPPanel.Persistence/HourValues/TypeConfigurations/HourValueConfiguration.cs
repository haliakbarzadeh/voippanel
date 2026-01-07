using Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettings.ValueObjects;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.Persistence.HourValues.TypeConfigurations;

public class HourValueConfiguration : IEntityTypeConfiguration<HourValue>
{
    public void Configure(EntityTypeBuilder<HourValue> builder)
    {

        builder.ToTable("HourValue");
    }
}