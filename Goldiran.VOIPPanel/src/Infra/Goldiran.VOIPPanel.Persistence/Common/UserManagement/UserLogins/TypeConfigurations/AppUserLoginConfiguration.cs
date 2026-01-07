using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.Persistence.Common.UserManagement.UserLogins.TypeConfigurations;

public class AppUserLoginConfiguration : IEntityTypeConfiguration<AppUserLogin>
{
    public void Configure(EntityTypeBuilder<AppUserLogin> builder)
    {
        builder.HasOne(userLogin => userLogin.User)
                              .WithMany(user => user.Logins)
                              //.WithMany()
               .HasForeignKey(userLogin => userLogin.UserId);

        builder.ToTable("UserLogins");
    }
}