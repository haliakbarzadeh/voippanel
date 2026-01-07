using Voip.Framework.EFCore.Common;
using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories;
using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Contracts;
using Goldiran.VOIPPanel.Persistence;

namespace Goldiran.VOIPPanel.Persistence.Positions;

public class PositionRepository : BaseRepository<long, Position>, IPositionRepository
{
    public PositionRepository(VOIPPanelContext context) : base(context)
    {
    }

}
