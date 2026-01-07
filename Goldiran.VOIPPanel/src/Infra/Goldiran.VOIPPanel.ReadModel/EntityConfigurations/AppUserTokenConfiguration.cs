using Goldiran.VOIPPanel.ReadModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.QueryHandler.EntityConfigurations;

public class AppUserTokenConfiguration : IEntityTypeConfiguration<AppUserToken>
{
    public void Configure(EntityTypeBuilder<AppUserToken> builder)
    {
        builder.HasKey(c => new { c.LoginProvider, c.Name,c.UserId });
        builder.HasOne(userLogin => userLogin.User)
       .WithMany(user => user.UserTokens)
       .HasForeignKey(userLogin => userLogin.UserId);
    }
}
