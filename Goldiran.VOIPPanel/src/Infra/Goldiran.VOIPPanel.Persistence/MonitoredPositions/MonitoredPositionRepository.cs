using Voip.Framework.EFCore.Common;
using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories;
using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.MonitoredPositions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.MonitoredPositions.Contracts;
using Goldiran.VOIPPanel.Persistence;

namespace Goldiran.VOIPPanel.Persistence.MonitoredPositions;

public class MonitoredPositionRepository : BaseRepository<long, MonitoredPosition>, IMonitoredPositionRepository
{
    public MonitoredPositionRepository(VOIPPanelContext context) : base(context)
    {
    }

}
