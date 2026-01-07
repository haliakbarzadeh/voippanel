using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories;
using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.Persistence.LoginHistories.TypeConfigurations;

public class LoginHistoryConfiguration : IEntityTypeConfiguration<LoginHistory>
{
    public void Configure(EntityTypeBuilder<LoginHistory> builder)
    {
        builder.HasOne<AppUser>()
            .WithMany()
            .HasForeignKey("UserId");
        builder.Property(c => c.ActionDate).IsRequired();
        builder.Property(c => c.LoginFunctionTypeId).IsRequired();

        builder.ToTable("LoginHistory");
    }
}