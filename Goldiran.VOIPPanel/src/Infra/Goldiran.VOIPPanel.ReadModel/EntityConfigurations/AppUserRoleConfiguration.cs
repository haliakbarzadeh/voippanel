using Goldiran.VOIPPanel.ReadModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.QueryHandler.EntityConfigurations;

public class AppUserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
{
    public void Configure(EntityTypeBuilder<AppUserRole> builder)
    {
        builder.HasKey(c=>new { c.UserId,c.RoleId});
        builder.HasOne(user => user.User)
           .WithMany(c=>c.UserRoles)
           .HasForeignKey(userLogin => userLogin.UserId);
        builder.HasOne(user => user.Role)
            .WithMany(c=>c.UserRoles)
            .HasForeignKey(userLogin => userLogin.RoleId);
    }
}
