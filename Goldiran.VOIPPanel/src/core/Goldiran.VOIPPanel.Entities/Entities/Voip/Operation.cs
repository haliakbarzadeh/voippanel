using Voip.Framework.Domain;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Entities;

public class Operation: BaseQueryEntity<long>
{
    public long UserId { get; set; }
    public AppUser User { get; set; }
    public long PositionId { get; set; }
    public Position Position { get; set; }
    public OperationType OperationTypeId { get; set; }
    public DateTime StartDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public DateTime? EndDate { get; set; }
    public TimeSpan? EndTime { get; set; }
    public bool IsCurrentStatus { get; set; }
    public string Queues { get;  set; }=string.Empty;
    public int Penalty { get; set; }
    public int StatusDuration { get; set; }
    public long? ManagerUserId { get; set; }
    public AppUser? ManagerUser { get; set; }
    public string? Reason { get; set; }


}
