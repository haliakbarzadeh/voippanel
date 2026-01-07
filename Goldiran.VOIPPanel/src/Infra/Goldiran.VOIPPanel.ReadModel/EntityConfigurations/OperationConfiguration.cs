using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories;
using Operation = Goldiran.VOIPPanel.ReadModel.Entities.Operation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations;

namespace Goldiran.VOIPPanel.QueryHandler.EntityConfigurations;

public class OperationConfiguration : IEntityTypeConfiguration<Operation>
{
    public void Configure(EntityTypeBuilder<Operation> builder)
    {
        builder.HasKey(e => e.Id);

        builder.ToTable("Operation");
    }
}