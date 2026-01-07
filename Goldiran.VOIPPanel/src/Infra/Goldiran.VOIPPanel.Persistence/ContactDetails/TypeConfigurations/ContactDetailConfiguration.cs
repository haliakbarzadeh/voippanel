using Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails;
using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.Persistence.ContactDetails.TypeConfigurations;

public class ContactDetailConfiguration : IEntityTypeConfiguration<ContactDetail>
{
    public void Configure(EntityTypeBuilder<ContactDetail> builder)
    {
        builder.Property(c => c.LinkedId).HasMaxLength(50).IsRequired();
        builder.Property(c => c.DstChannel).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Status).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Disposition).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Dcontext).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Source).HasMaxLength(50);
        builder.Property(c => c.Dest).HasMaxLength(50);
        builder.Property(c => c.Filepath).HasMaxLength(250);
        builder.Property(c => c.Recordingfile).HasMaxLength(250);

        builder.ToTable("ContactDetail");
    }
}