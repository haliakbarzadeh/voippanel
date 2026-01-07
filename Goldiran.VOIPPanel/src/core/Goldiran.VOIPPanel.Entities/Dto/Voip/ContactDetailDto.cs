using Voip.Framework.Domain.Extensions;
using Goldiran.VOIPPanel.Domain.Common.Attributes;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Common.Extensions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails.Enums;


namespace Goldiran.VOIPPanel.ReadModel.Dto;
public class ContactDetailDto
{
    [DtoAttributes("شناسه", false)]
    public string LinkedId { get; set; } = string.Empty;
    [DtoAttributes("شناسه", false)]
    public string DstChannel { get; set; } = string.Empty;
    [DtoAttributes("وضعیت تماس", false)]
    public string Status { get; set; } = string.Empty;
    [DtoAttributes("وضعیت تماس", false)]
    public string RealDisposition { get; set; } = string.Empty;
    //[DtoAttributes("وضعیت تماس", false)]
    //public string Disposition { get; set; } = string.Empty;
    [DtoAttributes("وضعیت تماس", true)]
    public string Disposition { get { return Duration != null && Duration == 0 && RealDisposition == "ANSWERED" ? "NO ANSWER" : RealDisposition; } }
    [DtoAttributes("شناسه", false)]
    public DateTime Date { get; set; }
    [DtoAttributes("تاریخ و زمان", false)]
    public string PersianDate { get { return Date.ConvertDateTimeToJalaliDateTime(); } }
    [DtoAttributes("تاریخ ", true)]
    public string PersianDateOnly { get { return PersianDate.Substring(0, PersianDate.IndexOf(' ')); } }
    [DtoAttributes("زمان ", true)]
    public string PersianTimeOnly { get { return PersianDate.Substring(PersianDate.IndexOf(' ') + 1); } }
    [DtoAttributes("شناسه", false)]
    public string Dcontext { get; set; } = string.Empty;
    [DtoAttributes("مبدا", true)]
    public string Source { get; set; } = string.Empty;
    [DtoAttributes("مقصد", true)]
    public string Dest { get; set; } = string.Empty;
    [DtoAttributes("شناسه", false)]
    public int Billsecond { get; set; }
    [DtoAttributes("مدت مکالمه", false)]
    public int Duration { get; set; }
    [DtoAttributes("مدت مکالمه", false)]
    public TimeSpan? DurationTimeSpan { get { return Duration != null ? TimeSpan.FromSeconds((int)Duration) : null; } }
    [DtoAttributes("مدت مکالمه", true)]
    public string DurationStr { get { return DurationTimeSpan != null ? $"{DurationTimeSpan.Value.Hours.ToString("00")}:{DurationTimeSpan.Value.Minutes.ToString("00")}:{DurationTimeSpan.Value.Seconds.ToString("00")}" : string.Empty; } }
    [DtoAttributes("زمان انتظار", false)]
    public int Waiting { get; set; }
    [DtoAttributes("زمان انتظار", false)]
    public TimeSpan? WaitingTimeSpan { get { return Waiting != null ? TimeSpan.FromSeconds((int)Waiting) : null; } }
    [DtoAttributes("زمان انتظار", true)]
    public string WaitingStr { get { return WaitingTimeSpan != null ? $"{WaitingTimeSpan.Value.Hours.ToString("00")}:{WaitingTimeSpan.Value.Minutes.ToString("00")}:{WaitingTimeSpan.Value.Seconds.ToString("00")}" : string.Empty; } }

    [DtoAttributes("شناسه", false)]
    public string Filepath { get; set; } = string.Empty;
    //public string RealFilepath { get { return !string.IsNullOrEmpty(Filepath) && Date.Date<DateTime.Now.Date ? Filepath.Replace(".WAV", ".mp3").Replace(".wav", ".mp3") : (!string.IsNullOrEmpty(Filepath)? Filepath .Replace(".wav",".WAV"): Filepath); } }
    [DtoAttributes("شناسه", false)]
    public string RealFilepath { get { return !string.IsNullOrEmpty(Filepath) ? ((Duration!=null && Duration!=0) ?Filepath.Replace(".WAV", ".mp3").Replace(".wav", ".mp3").Replace(".gsm", ".mp3"):string.Empty ): Filepath; } }
    [DtoAttributes("شناسه", false)]
    public string ShonoodFilePath { get { return !(RealDisposition== "ANSWERED" && Duration!=null && Duration==0) ?$"http://10.14.8.20/recording/{Date.Year.ToString("0000")}/{Date.Month.ToString("00")}/{Date.Day.ToString("00")}/{RealFilepath}":string.Empty; } }
    [DtoAttributes("شناسه", false)]
    public string Recordingfile { get; set; } = string.Empty;
    [DtoAttributes("شناسه", false)]
    public string? QueueName { get;  set; }


}
