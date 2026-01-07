using Goldiran.VOIPPanel.Domain.AggregatesModel.SoftPhoneEvents;
using Goldiran.VOIPPanel.Domain.AggregatesModel.SoftPhoneEvents.Contracts;
using Voip.Framework.EFCore.Common;

namespace Goldiran.VOIPPanel.Persistence.SoftPhoneEvents;

public class SoftPhoneEventsRepository : BaseRepository<long, SoftPhoneEvent>, ISoftPhoneEventsRepository
{
    public SoftPhoneEventsRepository(VOIPPanelContext dbContext) : base(dbContext)
    {
    }
}
