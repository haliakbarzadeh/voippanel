using Goldiran.VOIPPanel.ReadModel.Entities.Asterisk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.QueryHandler.EntityConfigurations;

public class CityNameConfiguration : IEntityTypeConfiguration<CityName>
{
    public void Configure(EntityTypeBuilder<CityName> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.CId).HasColumnName("c_id");
        builder.Property(e => e.Name).HasColumnName("c_name").HasColumnType("varchar(100)");
        builder.ToTable("marketingAD_cityName");
    }
}