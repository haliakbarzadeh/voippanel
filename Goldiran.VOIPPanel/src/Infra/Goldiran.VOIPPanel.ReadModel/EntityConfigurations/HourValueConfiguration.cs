using Goldiran.VOIPPanel.ReadModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.QueryHandler.EntityConfigurations;

public class HourValueConfiguration : IEntityTypeConfiguration<HourValue>
{
    public void Configure(EntityTypeBuilder<HourValue> builder)
    {
        builder.HasKey(e => e.Id);

        builder.ToTable("HourValue");
    }
}