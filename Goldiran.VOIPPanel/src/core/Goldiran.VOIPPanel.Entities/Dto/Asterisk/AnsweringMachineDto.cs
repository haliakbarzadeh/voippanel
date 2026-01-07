using Goldiran.VOIPPanel.Domain.AggregatesModel.AnsweringMachines.Enums;
using Goldiran.VOIPPanel.Domain.Common.Attributes;
using Goldiran.VOIPPanel.ReadModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;

public class AnsweringMachineDto
{
    [DtoAttributes("شناسه", false)]
    public long Id { get; set; }
    [DtoAttributes("شناسه", false)]
    public DateTime Date { get; set; }
    [DtoAttributes("شناسه", false)]
    public TimeSpan Time { get; set; }
    [DtoAttributes("تاریخ و زمان", true)]
    public string PersianDate { get { return (Date.Add(Time)).ConvertDateTimeToJalaliDateTime(); } }
    [DtoAttributes("تاریخ و زمان", false)]
    public int? Agent { get; set; }
    [DtoAttributes("تماس گیرنده", true)]
    public string CallerId { get; set; } = string.Empty;
    [DtoAttributes("نام تماس گیرنده", true)]
    public string CallerName { get; set; } = string.Empty;
    [DtoAttributes("توضیحات", true)]
    public string Description { get; set; } = string.Empty;
    [DtoAttributes("تاریخ و زمان", false)]
    public DateTime? EditDate { get; set; }
    [DtoAttributes("تاریخ و زمان", false)]
    public TimeSpan? EditTime { get; set; }
    [DtoAttributes("تاریخ و زمان", false)]
    public int Flag { get; set; }
    [DtoAttributes("موبایل", true)]
    public string Mob { get; set; } = string.Empty;
    [DtoAttributes("تلفن", true)]
    public string Phone { get; set; } = string.Empty;
    [DtoAttributes("تاریخ و زمان", false)]
    public string Queue { get; set; } = string.Empty;
    [DtoAttributes("تاریخ و زمان", false)]
    public string QueueName { get { return !string.IsNullOrEmpty(Queue) ? ((QueueTitle)Convert.ToInt32(Queue)).ToString() : string.Empty; } }
    [DtoAttributes("تاریخ و زمان", false)]
    public AMStatusType Status { get; set; }
    [DtoAttributes("وضعیت", true)]
    public string StatusStr { get { return Status.Description(); } }
    [DtoAttributes("تاریخ و زمان", false)]
    public DateTime? UpdateTime { get; set; }
    [DtoAttributes("زمان تغییر", true)]
    public string PersianUpdateDate { get { return (UpdateTime!=null && UpdateTime.Value.Year>2000)? UpdateTime.ConvertDateTimeToJalaliDateTime():string.Empty; } }
    [DtoAttributes("تغییر دهنده", true)]
    public string UserName { get; set; }=string.Empty;

}
