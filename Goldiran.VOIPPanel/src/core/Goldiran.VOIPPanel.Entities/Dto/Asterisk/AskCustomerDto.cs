using Goldiran.VOIPPanel.Domain.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;


public class AskCustomerDto
{
    [DtoAttributes("شناسه", false)]
    public long Id {  get; set; }
    [DtoAttributes("شناسه", false)]
    public DateTime Date { get; set; }
    [DtoAttributes("شماره داخلی", true)]
    public int Agent { get; set; }
    [DtoAttributes("شماره داخلی", false)]
    public TimeSpan Time {  get; set; }
    [DtoAttributes("تاریخ و زمان", false)]
    public string PersianDate { get { return  (Date.Add(Time)).ConvertDateTimeToJalaliDateTime() ; } }
    [DtoAttributes("تاریخ ", true)]
    public string PersianDateOnly { get { return PersianDate.Substring(0, PersianDate.IndexOf(' ')); } }
    [DtoAttributes("زمان ", true)]
    public string PersianTimeOnly { get { return PersianDate.Substring(PersianDate.IndexOf(' ') + 1); } }
    [DtoAttributes("شماره تماس", true)]
    public string CallerId { get; set; } = string.Empty;
    [DtoAttributes("صف", true)]
    public string Queue { get; set; } = string.Empty;
    [DtoAttributes("نمره", true)]
    public string Response {  get; set; } = string.Empty;
    [DtoAttributes("شماره داخلی", false)]
    public string UniqueId { get; set; } = string.Empty;
    [DtoAttributes("شماره داخلی", false)]
    public string CityNAme { get; set; } = string.Empty;
    [DtoAttributes("نام کاربر", true)]
    public string UserName { get; set; } = string.Empty;
    [DtoAttributes("شماره داخلی", false)]
    public Guid Guid { get; set; }



}
