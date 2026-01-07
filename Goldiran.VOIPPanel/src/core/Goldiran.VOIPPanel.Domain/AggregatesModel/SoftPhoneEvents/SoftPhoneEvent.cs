using Goldiran.VOIPPanel.Domain.AggregatesModel.SoftPhoneEvents.Enums;
using Voip.Framework.Domain;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.SoftPhoneEvents
{
    public class SoftPhoneEvent : AggregateRoot<long>
    {
        public SoftPhoneEvent(SoftPhoneEventType eventType, long userPositionId, long userId)
        {
            EventType = eventType;
            UserPositionId = userPositionId;
            UserId = userId;
            StartedAt = DateTime.Now;
        }

        public SoftPhoneEventType EventType { get; private set; }
        public long UserPositionId { get; private set; }
        public long UserId { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime? FinishedAt { get; private set; }


        public void Finished()
        {
            FinishedAt = DateTime.Now;
        }
    }
}
