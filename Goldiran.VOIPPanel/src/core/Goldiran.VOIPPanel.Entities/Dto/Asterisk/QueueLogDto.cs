using Goldiran.VOIPPanel.Domain.Common.Attributes;
using Goldiran.VOIPPanel.ReadModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Common.Extensions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;

public class QueueLogDto
{
    [DtoAttributes("شماره داخلی", true)]
    public string Agent { get; set; } = string.Empty;
    [DtoAttributes("شناسه", false)]
    public int CallDuration { get; set; }
    [DtoAttributes("شناسه", false)]
    public TimeSpan? CallDurationTimeSpan { get { return CallDuration != 0 ? TimeSpan.FromSeconds((int)CallDuration) : null; } }
    [DtoAttributes("مدت مکالمه", true)]
    public string CallDurationStr { get { return CallDurationTimeSpan != null ? $"{CallDurationTimeSpan.Value.Hours.ToString("00")}:{CallDurationTimeSpan.Value.Minutes.ToString("00")}:{CallDurationTimeSpan.Value.Seconds.ToString("00")}" : string.Empty; } }
    [DtoAttributes("وضعیت", true)]
    public string Event { get; set; } = string.Empty;
    [DtoAttributes("موقعیت هنگام ورود", true)]
    public string EntryPosition { get; set; } = string.Empty;
    [DtoAttributes("موقعیت در صف", true)]
    public string QueuePosition { get; set; } = string.Empty;
    [DtoAttributes("شناسه", false)]
    public int CallWaiting { get; set; }
    [DtoAttributes("شناسه", false)]
    public TimeSpan? CallWaitingTimeSpan { get { return CallWaiting != 0 ? TimeSpan.FromSeconds((int)CallWaiting) : null; } }
    [DtoAttributes("مدت انتظار", true)]
    public string CallWaitingStr { get { return CallWaitingTimeSpan != null ? $"{CallWaitingTimeSpan.Value.Hours.ToString("00")}:{CallWaitingTimeSpan.Value.Minutes.ToString("00")}:{CallWaitingTimeSpan.Value.Seconds.ToString("00")}" : string.Empty; } }
    [DtoAttributes("شماره صف", true)]
    public string QueueNumber { get; set; } = string.Empty;
    [DtoAttributes("عنوان صف", true)]
    public string QueueName { get { return !string.IsNullOrEmpty(QueueNumber) ? ((QueueTitle)Convert.ToInt32(QueueNumber)).ToString() : string.Empty; } }
    [DtoAttributes("شماره مشتری", true)]
    public string CallId { get; set; } = string.Empty;
    [DtoAttributes("شناسه", false)]
    public DateTime Created { get; set; }
    [DtoAttributes("تاریخ و زمان", false)]
    public string PersianDate { get { return Created.ConvertDateTimeToJalaliDateTime(); } }
    [DtoAttributes("زمان ", true)]
    public string PersianTimeOnly { get { return PersianDate.Substring(PersianDate.IndexOf(' ') + 1); } }
    [DtoAttributes("تاریخ ", true)]
    public string PersianDateOnly { get { return PersianDate.Substring(0, PersianDate.IndexOf(' ')); } }
    [DtoAttributes("شناسه", false)]
    public long Id { get; set; }
    [DtoAttributes("شناسه", false)]
    public string Data1 { get; set; } = string.Empty;
    [DtoAttributes("شناسه", false)]
    public string Data2 { get; set; } = string.Empty;
    [DtoAttributes("شناسه", false)]
    public string Data3 { get; set; } = string.Empty;
    [DtoAttributes("شناسه", false)]
    public string Data4 { get; set; } = string.Empty;
    [DtoAttributes("شناسه", false)]
    public string Data5 { get; set; } = string.Empty;
    [DtoAttributes("شناسه", false)]
    public string Time { get; set; } = string.Empty;



}
