using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.Persistence.Common.UserManagement.UserRoles.TypeConfigurations;

public class AppUserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
{
    public void Configure(EntityTypeBuilder<AppUserRole> builder)
    {
        builder.HasOne(userRole => userRole.Role)
               .WithMany(role => role.UserRoles)
               .HasForeignKey(userRole => userRole.RoleId);

        builder.HasOne(userRole => userRole.User)
               .WithMany(user => user.Roles)
               .HasForeignKey(userRole => userRole.UserId);

        builder.ToTable("UserRoles");
    }
}