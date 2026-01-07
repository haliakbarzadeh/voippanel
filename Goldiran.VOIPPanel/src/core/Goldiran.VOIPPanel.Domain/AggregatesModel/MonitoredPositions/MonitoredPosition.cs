using Voip.Framework.Domain;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.MonitoredPositions;

public class MonitoredPosition : AggregateRoot<long>
{
    public long SourcePositionId { get; private set; }
    //public string SourceNode { get; set; } = null!;
    public long DestPositionId { get; private set; }
    //public string DestNode { get; set; } = null!;
    public MonitoredPosition(long sourcePositionId, long destPositionId)
    {
        SourcePositionId = sourcePositionId;
        DestPositionId = destPositionId;
    }

    public void Upodate(long sourcePositionId, long destPositionId)
    {
        SourcePositionId = sourcePositionId;
        DestPositionId = destPositionId;
    }
}