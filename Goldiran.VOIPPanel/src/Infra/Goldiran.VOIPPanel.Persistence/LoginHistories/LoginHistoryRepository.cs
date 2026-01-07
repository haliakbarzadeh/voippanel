using Voip.Framework.EFCore.Common;
using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories;
using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories.Contracts;
using Goldiran.VOIPPanel.Persistence;

namespace Goldiran.VOIPPanel.Persistence.LoginHistories;

public class LoginHistoryRepository : BaseRepository<long, LoginHistory>, ILoginHistoryRepository
{
    public LoginHistoryRepository(VOIPPanelContext context) : base(context)
    {
    }

}
