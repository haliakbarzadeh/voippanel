using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations;
using Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.Persistence.LoginHistories.TypeConfigurations;

public class OperationSettingConfiguration : IEntityTypeConfiguration<OperationSetting>
{
    public void Configure(EntityTypeBuilder<OperationSetting> builder)
    {
        builder.HasIndex(c => c.OperationTypeId).IsUnique();
        builder.HasIndex(c => c.Order).IsUnique();
        builder.HasIndex(c => c.Name).IsUnique();
        builder.HasIndex(c => c.Label).IsUnique();
        //builder.OwnsOne(c=>c.ActiveTime,d=>d.ToJson()); 
        builder.OwnsOne(c => c.ActiveTime, d => {
            d.WithOwner();
            d.Property(p => p.StartTime).HasColumnName("StartTime");
            d.Property(p => p.EndTime).HasColumnName("EndTime");
        });

        builder.ToTable("OperationSetting");
    }
}