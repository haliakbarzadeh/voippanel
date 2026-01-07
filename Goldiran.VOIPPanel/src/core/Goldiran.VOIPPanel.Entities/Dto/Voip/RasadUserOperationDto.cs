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

public class RasadUserOperationDto
{
    public long UserId { get; set; }
    public long PositionId { get; set; }
    public string UserFullName {  get; set; }=string.Empty;
    public string Extension { get; set; } = string.Empty;
    public OperationType OperationTypeId { get; set; }
    public string OperationType { get { return OperationTypeId != 0 ? OperationTypeId.Description() : string.Empty; } }
    public DateTime StartDate { get; set; }
    public TimeSpan StartTime { get; set; }
    //public DateTime StartDateTime { get { return StartDate.Date == DateTime.Now.Date ? StartDate.Add(StartTime) : DateTime.Now.Date.Add(new TimeSpan(8, 30, 0)); } }
    public DateTime StartDateTime { get { return (StartDate.Date == DateTime.Now.Date || EndDate != null) ? StartDate.Add(StartTime) : DateTime.Now; } }
    public DateTime? EndDate { get; set; }
    public TimeSpan? EndTime { get; set; }
    public DateTime? EndDateTime { get { return EndDate != null ? ((DateTime)EndDate).Add((TimeSpan)EndTime) : null; } }
    public TimeSpan DurationTime { get { return EndDate != null ? ((DateTime)EndDate).Add((TimeSpan)EndTime) - StartDateTime : DateTime.Now - StartDateTime; } }
    // public TimeSpan DurationTime { get { return EndDate != null ? ((DateTime)EndDate).Add((TimeSpan)EndTime) - StartDate.Add(StartTime) : DateTime.Now - StartDate.Add(StartTime); } }
    public string StrDurationTime { get { return DurationTime.Days > 0 ? $"{DurationTime.Days}:{DurationTime.Hours.ToString("00")}:{DurationTime.Minutes.ToString("00")}:{DurationTime.Seconds.ToString("00")}" : $"{DurationTime.Hours.ToString("00")}:{DurationTime.Minutes.ToString("00")}:{DurationTime.Seconds.ToString("00")}"; } }
    public long Duration { get { return (long)DurationTime.TotalMinutes; } }
    public TimeSpan TimeDuration { get { return new TimeSpan(0, (int)Duration, 0); } }
    public string StrDuration { get { return IsAnswering ?AnsweringDuration: (DurationTime.Days > 0 ? $"{DurationTime.Days}:{DurationTime.Hours.ToString("00")}:{DurationTime.Minutes.ToString("00")}:{DurationTime.Seconds.ToString("00")}" : $"{DurationTime.Hours.ToString("00")}:{DurationTime.Minutes.ToString("00")}:{DurationTime.Seconds.ToString("00")}"); } }
    public string Queue { get; set; }=string.Empty;
    public string Caller { get; set; } = string.Empty;
    public bool IsAnswering {  get; set; }
    public string AnsweringDuration { get; set; }

}
