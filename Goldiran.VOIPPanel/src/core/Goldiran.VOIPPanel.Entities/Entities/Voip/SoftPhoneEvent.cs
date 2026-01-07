using Goldiran.VOIPPanel.Domain.AggregatesModel.SoftPhoneEvents.Enums;
using Voip.Framework.Domain;

namespace Goldiran.VOIPPanel.ReadModel.Entities.Voip;

public class SoftPhoneEvent : BaseQueryEntity<long>
{
    public SoftPhoneEventType EventType { get; set; }
    public long UserPositionId { get; set; }
    public long UserId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public virtual UserPosition UserPosition { get; set; }
    public virtual AppUser User { get; set; }
}
