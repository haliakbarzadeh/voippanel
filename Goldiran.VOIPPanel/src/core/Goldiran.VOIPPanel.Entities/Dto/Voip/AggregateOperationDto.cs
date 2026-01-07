using Voip.Framework.Domain.Extensions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.ReadModel.Dto;

public class AggregateOperationDto
{
    public long UserId { get; set; }
    public long PositionId { get; set; }
    public string PersianFullName { get; set; } = string.Empty;
    public OperationType OperationTypeId { get; set; }
    public string OperationType { get { return OperationTypeId != 0 ? OperationTypeId.Description() : string.Empty; } }
    public int Count { get; set; }
    public long Duration { get; set; }
    public TimeSpan TimeDuration { get { return Duration<1440? new TimeSpan(0, (int)Duration, 0): new TimeSpan(0, (int)(Duration%1440), 0); } }
    public string StrDuration { get { return TimeDuration.Days>0? $"{TimeDuration.Days}:{TimeDuration.Hours.ToString("00")}:{TimeDuration.Minutes.ToString("00")}:{TimeDuration.Seconds.ToString("00")}": $"{TimeDuration.Hours.ToString("00")}:{TimeDuration.Minutes.ToString("00")}:{TimeDuration.Seconds.ToString("00")}"; } }
    public int? MojazDuration { get; set; }
    public TimeSpan TimeMojazDuration { get { return MojazDuration!=null? new TimeSpan(0, (int)MojazDuration, 0): new TimeSpan(0, 0, 0); } }
    public string StrMojaDuration { get { return TimeMojazDuration.TotalMinutes>0?( TimeMojazDuration.Days > 0 ? $"{TimeMojazDuration.Days}:{TimeMojazDuration.Hours.ToString("00")}:{TimeMojazDuration.Minutes.ToString("00")}:{TimeMojazDuration.Seconds.ToString("00")}" : $"{TimeMojazDuration.Hours.ToString("00")}:{TimeMojazDuration.Minutes.ToString("00")}:{TimeMojazDuration.Seconds.ToString("00")}"):string.Empty; } }
    public int? MojazCount { get; set; }
    //public bool IsCurrentStatus { get; set; }
}
