using Goldiran.VOIPPanel.ReadModel.Entities.Asterisk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.QueryHandler.EntityConfigurations;

public class AskCustomerConfiguration : IEntityTypeConfiguration<AskCustomer>
{
    public void Configure(EntityTypeBuilder<AskCustomer> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Date).HasColumnName("aDate").HasColumnType("date");
        builder.Property(e => e.Agent).HasColumnName("agent");
        builder.Property(e => e.Time).HasColumnName("aTime").HasColumnType("time");
        builder.Property(e => e.CallerId).HasColumnName("callerid").HasColumnType("varchar(30)");
        builder.Property(e => e.Queue).HasColumnName("queue").HasColumnType("varchar(10)");
        builder.Property(e => e.Response).HasColumnName("responce").HasColumnType("varchar(10)");
        builder.Property(e => e.UniqueId).HasColumnName("uniqueId").HasColumnType("varchar(50)");

        builder.ToTable("askcustomer");
    }
}