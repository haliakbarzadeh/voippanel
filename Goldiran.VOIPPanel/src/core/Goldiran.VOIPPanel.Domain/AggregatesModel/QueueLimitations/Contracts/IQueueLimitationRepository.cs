using Voip.Framework.Domain;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitationSettings.Contracts;

public interface IQueueLimitationRepository : IBaseRepository<int, QueueLimitation>
{
    public void RemoveHourValues(QueueLimitation queueLimitation);
    public void RemoveHourValue(HourValue hourValue);
    public void UpdateHourValue(HourValue hourValue);
    public Task<HourValue> FindHourValue(int id);


}
