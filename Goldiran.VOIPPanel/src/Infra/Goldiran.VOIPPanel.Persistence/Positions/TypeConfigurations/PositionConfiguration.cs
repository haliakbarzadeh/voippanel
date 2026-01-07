using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.Persistence.Positions.TypeConfigurations;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        //builder.HasOne<parentposition>()
        //    .WithMany()
        //    .HasForeignKey("ParentPositionId");
        builder.HasIndex(c =>  c.Title, "IX_Position").IsUnique();
        builder.Property(c => c.Title).HasMaxLength(250).IsRequired();
        builder.Property(c => c.PositionTypeId).IsRequired();
        builder.Property(c => c.ContactedParentPositionId).HasMaxLength(250);
        builder.Property(c => c.ContactedParentPositionName).HasMaxLength(400);

        builder.ToTable("Position");
    }
}