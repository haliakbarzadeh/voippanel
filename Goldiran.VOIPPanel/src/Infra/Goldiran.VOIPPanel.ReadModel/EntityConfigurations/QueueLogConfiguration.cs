using Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;
using Goldiran.VOIPPanel.ReadModel.Entities.Asterisk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.QueryHandler.EntityConfigurations;

public class QueueLogConfigurationConfiguration : IEntityTypeConfiguration<QueueLog>
{
    public void Configure(EntityTypeBuilder<QueueLog> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Agent).HasColumnName("agent").HasColumnType("varchar(20)");
        builder.Property(e => e.CallId).HasColumnName("callid").HasColumnType("varchar(40)");
        builder.Property(e => e.Created).HasColumnName("created").HasColumnType("timestamp");
        builder.Property(e => e.Data1).HasColumnName("data1").HasColumnType("varchar(40)");
        builder.Property(e => e.Data2).HasColumnName("data2").HasColumnType("varchar(40)");
        builder.Property(e => e.Data3).HasColumnName("data3").HasColumnType("varchar(40)");
        builder.Property(e => e.Data4).HasColumnName("data4").HasColumnType("varchar(40)");
        builder.Property(e => e.Data5).HasColumnName("data5").HasColumnType("varchar(40)");
        builder.Property(e => e.Event).HasColumnName("event").HasColumnType("varchar(20)");
        builder.Property(e => e.QueueNumber).HasColumnName("queuename").HasColumnType("varchar(20)");
        builder.Property(e => e.Time).HasColumnName("time").HasColumnType("varchar(26)");

        builder.ToTable("queue_log");
    }
}