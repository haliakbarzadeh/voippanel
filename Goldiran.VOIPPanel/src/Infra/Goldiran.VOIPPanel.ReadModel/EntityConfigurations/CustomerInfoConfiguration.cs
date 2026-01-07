using Goldiran.VOIPPanel.ReadModel.Entities.Asterisk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.QueryHandler.EntityConfigurations;

public class CustomerInfoConfiguration : IEntityTypeConfiguration<CustomerInfo>
{
    public void Configure(EntityTypeBuilder<CustomerInfo> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Region).HasColumnName("region");
        builder.Property(e => e.CityNameId).HasColumnName("cityid").HasColumnType("varchar(100)");
        builder.Property(e => e.CustomerCD).HasColumnName("customerCD").HasColumnType("varchar(100)");
        builder.Property(e => e.Number).HasColumnName("number").HasColumnType("varchar(100)");
        builder.ToTable("marketingAD_customerCD");
    }
}