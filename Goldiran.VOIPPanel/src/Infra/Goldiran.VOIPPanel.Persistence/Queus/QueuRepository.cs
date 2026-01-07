using Voip.Framework.EFCore.Common;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitationSettings.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus.Contracts;
using Goldiran.VOIPPanel.Persistence;

namespace Goldiran.VOIPPanel.Persistence.QueueLimitations;

public class QueuRepository : BaseRepository<int, Queu>, IQueuRepository
{
    public QueuRepository(VOIPPanelContext context) : base(context)
    {
    }

}
