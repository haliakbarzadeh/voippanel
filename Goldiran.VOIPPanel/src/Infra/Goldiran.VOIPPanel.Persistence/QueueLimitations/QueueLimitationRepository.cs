using Voip.Framework.EFCore.Common;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitationSettings.Contracts;
using Goldiran.VOIPPanel.Persistence;

namespace Goldiran.VOIPPanel.Persistence.QueueLimitations;

public class QueueLimitationRepository : BaseRepository<int, QueueLimitation>, IQueueLimitationRepository
{
    public QueueLimitationRepository(VOIPPanelContext context) : base(context)
    {
    }


    public void RemoveHourValue(HourValue hourValue)
    {
        DbContext.RemoveRange(hourValue);
    }

    public void RemoveHourValues(QueueLimitation queueLimitation)
    {
        DbContext.RemoveRange(queueLimitation.HourValues);
    }

    public void UpdateHourValue(HourValue hourValue)
    {
        DbContext.Update(hourValue);
    }

    public async Task<HourValue> FindHourValue(int id)
    {
        return await DbContext.FindAsync<HourValue>(id);
    }
}
