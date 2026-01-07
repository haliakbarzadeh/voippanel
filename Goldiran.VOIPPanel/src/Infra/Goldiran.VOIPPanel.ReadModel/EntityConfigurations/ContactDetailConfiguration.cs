using Goldiran.VOIPPanel.ReadModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.QueryHandler.EntityConfigurations;

public class ContactDetailConfiguration : IEntityTypeConfiguration<ContactDetail>
{
    public void Configure(EntityTypeBuilder<ContactDetail> builder)
    {
        builder.HasKey(e => e.Id);

        builder.ToTable("ContactDetail");
    }
}