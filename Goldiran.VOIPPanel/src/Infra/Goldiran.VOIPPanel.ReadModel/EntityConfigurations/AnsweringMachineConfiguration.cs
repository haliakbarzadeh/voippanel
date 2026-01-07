using Goldiran.VOIPPanel.ReadModel.Entities.Asterisk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.QueryHandler.EntityConfigurations;

public class AnsweringMachineConfiguration : IEntityTypeConfiguration<AnsweringMachine>
{
    public void Configure(EntityTypeBuilder<AnsweringMachine> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Date).HasColumnName("am_date").HasColumnType("date");
        builder.Property(e => e.Agent).HasColumnName("agent");
        builder.Property(e => e.Time).HasColumnName("am_time").HasColumnType("time");
        builder.Property(e => e.CallerId).HasColumnName("callerid").HasColumnType("varchar(20)");
        builder.Property(e => e.Queue).HasColumnName("queue").HasColumnType("varchar(10)");
        builder.Property(e => e.CallerName).HasColumnName("callerName").HasColumnType("varchar(100)");
        builder.Property(e => e.Description).HasColumnName("describe").HasColumnType("varchar(500)");
        builder.Property(e => e.EditDate).HasColumnName("edit_date").HasColumnType("date");
        builder.Property(e => e.EditTime).HasColumnName("edit_time").HasColumnType("time");
        builder.Property(e => e.Flag).HasColumnName("flag");
        builder.Property(e => e.Mob).HasColumnName("mob");
        builder.Property(e => e.Phone).HasColumnName("phone");
        builder.Property(e => e.Queue).HasColumnName("queue");
        builder.Property(e => e.Status).HasColumnName("status");
        builder.Property(e => e.UpdateTime).HasColumnName("update-time").HasColumnType("datetime");

        builder.ToTable("AM");
    }
}