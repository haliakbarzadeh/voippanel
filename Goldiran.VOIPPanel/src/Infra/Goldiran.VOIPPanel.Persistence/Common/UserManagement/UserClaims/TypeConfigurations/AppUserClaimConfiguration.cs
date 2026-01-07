using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.Persistence.Common.UserManagement.UserClaims.TypeConfigurations;

public class AppUserClaimConfiguration : IEntityTypeConfiguration<AppUserClaim>
{
    public void Configure(EntityTypeBuilder<AppUserClaim> builder)
    {
        builder.HasOne(userClaim => userClaim.User)
               .WithMany(user => user.Claims)
               .HasForeignKey(userClaim => userClaim.UserId);

        builder.ToTable("UserClaims");
    }
}
