using Voip.Framework.EFCore.Common;
using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories;
using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions.Contracts;
using Goldiran.VOIPPanel.Persistence;

namespace Goldiran.VOIPPanel.Persistence.UserPositions;

public class UserPositionRepository : BaseRepository<long, UserPosition>, IUserPositionRepository
{
    public UserPositionRepository(VOIPPanelContext context) : base(context)
    {
    }

}
