using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.Persistence.LoginHistories.TypeConfigurations;

public class QueueLimitationConfiguration : IEntityTypeConfiguration<QueueLimitation>
{
    public void Configure(EntityTypeBuilder<QueueLimitation> builder)
    {
        //builder.OwnsMany(c => c.HourValues, d =>
        //{
        //    d.WithOwner().HasForeignKey("QueueLimitationId");
        //    d.Property<int>("Id");
        //    d.HasKey("Id");
        //    d.Property(h => h.HourType).IsRequired(); 
        //    d.Property(h => h.Value).IsRequired();
        //    d.ToTable("HourValue");
        //});
        builder.HasOne<Queu>()
            .WithMany()
            .HasForeignKey("QueueId");
        builder.ToTable("QueueLimitation");
    }
}