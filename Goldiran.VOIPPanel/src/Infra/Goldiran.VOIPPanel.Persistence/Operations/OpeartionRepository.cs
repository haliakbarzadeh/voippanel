using Voip.Framework.EFCore.Common;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Contracts;
using Goldiran.VOIPPanel.Persistence;

namespace Goldiran.VOIPPanel.Persistence.Operations;

public class OperationRepository : BaseRepository<long, Operation>, IOperationRepository
{
    public OperationRepository(VOIPPanelContext context) : base(context)
    {
    }

}
