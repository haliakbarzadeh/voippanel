using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.Persistence.Common.UserManagement.Users.TypeConfigurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasIndex(c=>c.UserName).IsUnique();
        builder.HasIndex(c => c.NationalCode).IsUnique();
        builder.Property(t => t.UserName)
            .HasMaxLength(256)
            .IsRequired();
        builder.Property(t => t.Email)
            .HasMaxLength(256)
            .IsRequired();
        builder.Property(t => t.NationalCode)
            .HasMaxLength(10)
            .IsRequired();
        builder.Property(t => t.PersianFullName)
            .HasMaxLength(100);
        builder.Property(t => t.LatinFullName)
            .HasMaxLength(100);
        builder.Property(t => t.Address)
            .HasMaxLength(256);
       // builder.HasMany<AppUserLogin>(user => user.Logins)
       //.WithOne()
       //.HasForeignKey(userLogin => userLogin.UserId);

        builder.ToTable("User");
    }
}
