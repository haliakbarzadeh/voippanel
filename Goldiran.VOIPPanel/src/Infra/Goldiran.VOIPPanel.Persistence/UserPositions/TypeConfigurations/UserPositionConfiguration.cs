using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions;
using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.Persistence.LoginHistories.TypeConfigurations;

public class UserPositionConfiguration : IEntityTypeConfiguration<UserPosition>
{
    public void Configure(EntityTypeBuilder<UserPosition> builder)
    {
        builder.HasOne<AppUser>()
            .WithMany()
            .HasForeignKey("UserId");

        builder.HasOne<Position>()
            .WithMany()
            .HasForeignKey("PositionId");

        builder.Property(c => c.StartDate).IsRequired();
        builder.Property(c => c.Queues).HasMaxLength(400);
        builder.Property(c => c.Extension).HasMaxLength(400);
        builder.Property(c => c.ServerIp).HasMaxLength(400);

        builder.ToTable("UserPosition");
    }
}