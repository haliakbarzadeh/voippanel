using Voip.Framework.Domain;

namespace Goldiran.VOIPPanel.ReadModel.Entities;

public class MonitoredPosition : BaseQueryEntity<long>
{
    public long SourcePositionId { get; set; }
    public Position SourcePosition { get; set; }
    //public string SourceNode { get; set; } = null!;
    public long DestPositionId { get; set; }
    public Position DestPosition { get; set; }
    //public string DestNode { get; set; } = null!;
}