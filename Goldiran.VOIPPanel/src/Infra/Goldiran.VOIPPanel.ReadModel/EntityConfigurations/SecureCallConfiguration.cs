using Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;
using Goldiran.VOIPPanel.ReadModel.Entities.Asterisk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.QueryHandler.EntityConfigurations;

public class SecureCallConfiguration : IEntityTypeConfiguration<SecureCall>
{
    public void Configure(EntityTypeBuilder<SecureCall> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.SessionId).HasColumnName("session_id");
        builder.Property(e => e.Date).HasColumnName("date").HasColumnType("datetime");
        builder.Property(e => e.UniqueId).HasColumnName("uniqueid").HasColumnType("varchar(50)");
        builder.Property(e => e.CustomerNumber).HasColumnName("ccall").HasColumnType("varchar(20)");
        builder.Property(e => e.CustomerCallStatus).HasColumnName("ccall_status").HasColumnType("varchar(20)");
        builder.Property(e => e.Duration).HasColumnName("duration").HasColumnType("varchar(20)");
        builder.Property(e => e.ErrorMessage).HasColumnName("error_msg").HasColumnType("varchar(255)");
        builder.Property(e => e.ServiceNo).HasColumnName("serviceno").HasColumnType("varchar(200)");
        builder.Property(e => e.TechNumber).HasColumnName("tcall").HasColumnType("varchar(20)");
        builder.Property(e => e.TechCallStatus).HasColumnName("tcall_status").HasColumnType("varchar(20)");
        builder.Property(e => e.Type).HasColumnName("type").HasColumnType("tinyint");

        builder.ToTable("call_log");
    }
}