using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Goldiran.VOIPPanel.Persistence.Common.UserManagement.UserTokens.TypeConfigurations;

public class AppUserTokenConfiguration : IEntityTypeConfiguration<AppUserToken>
{
    public void Configure(EntityTypeBuilder<AppUserToken> builder)
    {
        builder.HasOne(userToken => userToken.User)
                              //.WithMany()
                              .WithMany(user=>user.UserTokens)
               .HasForeignKey(userToken => userToken.UserId);

        builder.ToTable("AppUserTokens");
    }
}