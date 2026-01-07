using Goldiran.VOIPPanel.ReadModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.QueryHandler.EntityConfigurations;

public class QueueLimitationConfiguration : IEntityTypeConfiguration<QueueLimitation>
{
    public void Configure(EntityTypeBuilder<QueueLimitation> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasOne<Queu>(c=>c.Queue)
        .WithMany()
        .HasForeignKey("QueueId");
        builder.ToTable("QueueLimitation");
    }
}