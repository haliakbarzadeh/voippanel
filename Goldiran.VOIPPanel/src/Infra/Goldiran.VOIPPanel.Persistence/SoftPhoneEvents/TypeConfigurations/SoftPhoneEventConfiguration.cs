using Goldiran.VOIPPanel.Domain.AggregatesModel.SoftPhoneEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goldiran.VOIPPanel.Persistence.SoftPhoneEvents.TypeConfigurations
{
    internal class SoftPhoneEventConfiguration : IEntityTypeConfiguration<SoftPhoneEvent>
    {
        public void Configure(EntityTypeBuilder<SoftPhoneEvent> builder)
        {
            builder.ToTable("SoftPhoneEvent");
        }
    }
}
