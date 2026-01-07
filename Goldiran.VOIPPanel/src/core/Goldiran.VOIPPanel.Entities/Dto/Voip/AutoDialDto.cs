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
public class AutoDialDto
{
    [DtoAttributes("شناسه", false)]
    public string LinkedId { get; set; } = string.Empty;
    [DtoAttributes("شناسه", false)]
    public string DstChannel { get; set; } = string.Empty;
    [DtoAttributes("وضعیت تماس", false)]
    public string Status { get; set; } = string.Empty;
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
    [DtoAttributes("وضعیت تماس کاربر", true)]
    public string RealDisposition { get; set; } = string.Empty;
    [DtoAttributes("وضعیت تماس مشتری", true)]
    public string CustomerStatus { get; set; } = string.Empty;
    [DtoAttributes("شناسه", false)]
    public int Billsecond { get; set; }
    [DtoAttributes("مدت مکالمه", false)]
    public int Duration { get; set; }
    [DtoAttributes("مدت مکالمه", false)]
    public TimeSpan? DurationTimeSpan { get { return Duration != null ? TimeSpan.FromSeconds((int)Duration) : null; } }
    [DtoAttributes("مدت مکالمه", true)]
    public string DurationStr { get { return DurationTimeSpan != null ? $"{DurationTimeSpan.Value.Hours.ToString("00")}:{DurationTimeSpan.Value.Minutes.ToString("00")}:{DurationTimeSpan.Value.Seconds.ToString("00")}" : string.Empty; } }
    [DtoAttributes("زمان انتظار", false)]
    public int CustomWaiting { get; set; }
    [DtoAttributes("زمان انتظار مشتری", false)]
    public TimeSpan? CustomWaitingTimeSpan { get { return CustomWaiting != null ? TimeSpan.FromSeconds((int)CustomWaiting) : null; } }
    [DtoAttributes("زمان انتظار مشتری", true)]
    public string CustomWaitingStr { get { return CustomWaitingTimeSpan != null ? $"{CustomWaitingTimeSpan.Value.Hours.ToString("00")}:{CustomWaitingTimeSpan.Value.Minutes.ToString("00")}:{CustomWaitingTimeSpan.Value.Seconds.ToString("00")}" : string.Empty; } }
    [DtoAttributes("زمان انتظار", false)]
    public int AgentWaiting { get; set; }
    [DtoAttributes("زمان انتظار", false)]
    public TimeSpan? AgentWaitingTimeSpan { get { return AgentWaiting != null ? TimeSpan.FromSeconds((int)AgentWaiting) : null; } }
    [DtoAttributes("زمان انتظار کاربر", true)]
    public string AgentWaitingStr { get { return AgentWaitingTimeSpan != null ? $"{AgentWaitingTimeSpan.Value.Hours.ToString("00")}:{AgentWaitingTimeSpan.Value.Minutes.ToString("00")}:{AgentWaitingTimeSpan.Value.Seconds.ToString("00")}" : string.Empty; } }
    [DtoAttributes("زمان انتظار", false)]
    public int CustomToAgentWaiting { get; set; }
    [DtoAttributes("زمان انتظار", false)]
    public TimeSpan? CustomToAgentWaitingTimeSpan { get { return CustomToAgentWaiting != null ? TimeSpan.FromSeconds((int)CustomToAgentWaiting) : null; } }
    [DtoAttributes("زمان انتظار مشتری برای کاربر", true)]
    public string CustomToAgentWaitingStr { get { return CustomToAgentWaitingTimeSpan != null ? $"{CustomToAgentWaitingTimeSpan.Value.Hours.ToString("00")}:{CustomToAgentWaitingTimeSpan.Value.Minutes.ToString("00")}:{CustomToAgentWaitingTimeSpan.Value.Seconds.ToString("00")}" : string.Empty; } }
    [DtoAttributes("زمان انتظار", false)]
    public int AgentToCustomWaiting { get; set; }
    [DtoAttributes("زمان انتظار", false)]
    public TimeSpan? AgentToCustomWaitingTimeSpan { get { return AgentToCustomWaiting != null ? TimeSpan.FromSeconds((int)AgentToCustomWaiting) : null; } }
    [DtoAttributes("زمان انتظار کاربر برای مشتری", true)]
    public string AgentToCustomWaitingStr { get { return AgentToCustomWaitingTimeSpan != null ? $"{AgentToCustomWaitingTimeSpan.Value.Hours.ToString("00")}:{AgentToCustomWaitingTimeSpan.Value.Minutes.ToString("00")}:{AgentToCustomWaitingTimeSpan.Value.Seconds.ToString("00")}" : string.Empty; } }
    [DtoAttributes("شناسه", false)]
    public string Filepath { get; set; } = string.Empty;
    //public string RealFilepath { get { return !string.IsNullOrEmpty(Filepath) && Date.Date<DateTime.Now.Date ? Filepath.Replace(".WAV", ".mp3").Replace(".wav", ".mp3") : (!string.IsNullOrEmpty(Filepath)? Filepath .Replace(".wav",".WAV"): Filepath); } }
    [DtoAttributes("شناسه", false)]
    public string RealFilepath { get { return !string.IsNullOrEmpty(Filepath) ? (Filepath.Replace(".WAV", ".mp3").Replace(".wav", ".mp3").Replace(".gsm", ".mp3")): Filepath; } }
    [DtoAttributes("شناسه", false)]
    public string ShonoodFilePath { get { return (!string.IsNullOrEmpty(RealFilepath) ) ?$"http://10.14.8.20/recording/{Date.Year.ToString("0000")}/{Date.Month.ToString("00")}/{Date.Day.ToString("00")}/{RealFilepath}":string.Empty; } }
    [DtoAttributes("شناسه", false)]
    public string Recordingfile { get; set; } = string.Empty;
    [DtoAttributes("شناسه", false)]
    public string? QueueName { get;  set; }


}
