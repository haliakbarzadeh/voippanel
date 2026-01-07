using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.Persistence.Common.UserManagement.RoleClaims.TypeConfigurations;

public class AppRoleClaimConfiguration : IEntityTypeConfiguration<AppRoleClaim>
{
    public void Configure(EntityTypeBuilder<AppRoleClaim> builder)
    {
        builder.HasOne(roleClaim => roleClaim.Role)
               .WithMany(role => role.Claims)
               .HasForeignKey(roleClaim => roleClaim.RoleId);

        builder.ToTable("RoleClaims");
    }
}