using Goldiran.VOIPPanel.ReadModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.QueryHandler.EntityConfigurations;

public class AppUserLoginConfiguration : IEntityTypeConfiguration<AppUserLogin>
{
    public void Configure(EntityTypeBuilder<AppUserLogin> builder)
    {
        builder.HasKey(c => new { c.LoginProvider, c.ProviderKey });
        builder.HasOne(userLogin => userLogin.User)
       .WithMany(user => user.Logins)
       .HasForeignKey(userLogin => userLogin.UserId);
    }
}
