using Goldiran.VOIPPanel.ReadModel.Entities.Asterisk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.QueryHandler.EntityConfigurations;

public class RegionNameConfiguration : IEntityTypeConfiguration<RegionName>
{
    public void Configure(EntityTypeBuilder<RegionName> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.CityNameId).HasColumnName("cityID");
        builder.Property(e => e.Name).HasColumnName("name").HasColumnType("varchar(100)");
        builder.ToTable("marketingAD_regionName");
    }
}