using Voip.Framework.Domain.Extensions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Enums;
using Goldiran.VOIPPanel.Domain.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.ReadModel.Dto;

public class OperationDto
{
    [DtoAttributes("داخلی", true)]
    public string? Extension { get; set; }
    [DtoAttributes("شناسه", false)]
    public long UserId { get; set; }
    [DtoAttributes("شناسه", false)]
    public long PositionId { get; set; }
    [DtoAttributes("نام داخلی", true)]
    public string UserFullName {  get; set; }=string.Empty;
    [DtoAttributes("شناسه", false)]
    public OperationType OperationTypeId { get; set; }
    [DtoAttributes("فعالیت", true)]
    public string OperationType { get { return OperationTypeId != 0 ? OperationTypeId.Description() : string.Empty; } }
    [DtoAttributes("شناسه", false)]
    public DateTime StartDate { get; set; }
    [DtoAttributes("شناسه", false)]
    public TimeSpan StartTime { get; set; }
    //public DateTime StartDateTime { get { return StartDate.Date==DateTime.Now.Date? StartDate.Add(StartTime): DateTime.Now.Date.Add(new TimeSpan(8,30,0)); } }
    [DtoAttributes("تاریخ شروع", false)]
    public DateTime StartDateTime { get { return ((StartDate.Date == DateTime.Now.Date || EndDate!=null) || OperationTypeId== Goldiran.VOIPPanel.Domain.AggregatesModel.Files.OperationType.Exit) ? StartDate.Add(StartTime) : DateTime.Now; } }
    [DtoAttributes("تاریخ شروع", false)]
    public string PersianStartDate { get { return StartDateTime.ConvertDateTimeToJalaliDateTime(); } }
    [DtoAttributes("تاریخ شروع", true)]
    public string PersianStartDateDash { get { return StartDateTime.ConvertDateTimeToJalaliDateTimeDash(); } }
    [DtoAttributes("شناسه", false)]
    public DateTime? EndDate { get; set; }
    [DtoAttributes("شناسه", false)]
    public TimeSpan? EndTime { get; set; }
    [DtoAttributes("شناسه", false)]
    public DateTime? EndDateTime { get { return EndDate!=null? ((DateTime)EndDate).Add((TimeSpan)EndTime):null; } }
    [DtoAttributes("تاریخ پایان", true)]
    public string PersianEndDateDash { get { return EndDateTime != null ? ((DateTime)EndDateTime).ConvertDateTimeToJalaliDateTimeDash() : string.Empty; } }
    [DtoAttributes("تاریخ پایان", false)]
    public string PersianEndDate { get { return EndDateTime != null ? ((DateTime)EndDateTime).ConvertDateTimeToJalaliDateTime() : string.Empty; } }
    [DtoAttributes("شناسه", false)]
    public bool IsCurrentStatus { get; set; }
    [DtoAttributes("شناسه", false)]
    public TimeSpan DurationTime { get { return EndDate != null ? ((DateTime)EndDate).Add((TimeSpan)EndTime) - StartDateTime : DateTime.Now - StartDateTime; } }
    [DtoAttributes("مدت زمان", true)]
    public string StrDurationTime { get { return DurationTime.Days > 0 ? $"{DurationTime.Days}:{DurationTime.Hours.ToString("00")}:{DurationTime.Minutes.ToString("00")}:{DurationTime.Seconds.ToString("00")}" : $"{DurationTime.Hours.ToString("00")}:{DurationTime.Minutes.ToString("00")}:{DurationTime.Seconds.ToString("00")}"; } }
    [DtoAttributes("شناسه", false)]
    public long Duration { get { return (long)DurationTime.TotalMinutes; } }
    [DtoAttributes("شناسه", false)]
    public string Queues { get; set; }=string.Empty;
    [DtoAttributes("پنالتی", true)]
    public int Penalty { get; set; }
    [DtoAttributes("شناسه", false)]
    public int StatusDuration { get; set; }
    [DtoAttributes("نام کاربر", true)]
    public string PersianFullName { get; set; } = string.Empty;
    [DtoAttributes("شناسه", false)]
    public long? ManagerUserId { get; set; }
    [DtoAttributes("تغییر وضعیت توسط", true)]
    public string ManagerUserFullName { get; set; } = string.Empty;
    [DtoAttributes("دلیل تغییر وضعیت", true)]
    public string? Reason { get; set; }
    [DtoAttributes("مدت زمان", false)]
    public string StrDuration { get { return DurationTime.Days > 0 ? $"{DurationTime.Days}:{DurationTime.Hours.ToString("00")}:{DurationTime.Minutes.ToString("00")}:{DurationTime.Seconds.ToString("00")}" : $"{DurationTime.Hours.ToString("00")}:{DurationTime.Minutes.ToString("00")}:{DurationTime.Seconds.ToString("00")}"; } }
}
