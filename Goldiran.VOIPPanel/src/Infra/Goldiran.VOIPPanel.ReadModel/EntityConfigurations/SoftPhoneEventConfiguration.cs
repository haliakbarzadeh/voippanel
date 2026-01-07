using Goldiran.VOIPPanel.ReadModel.Entities.Voip;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.QueryHandler.EntityConfigurations;

public class SoftPhoneEventConfiguration:IEntityTypeConfiguration<SoftPhoneEvent>
{
    public void Configure(EntityTypeBuilder<SoftPhoneEvent> builder)
    {
        builder.HasKey(e => e.Id);

        builder.ToTable("SoftPhoneEvent");
    }
}