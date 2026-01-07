using Goldiran.Framework.Domain.Extensions;

namespace Goldiran.VOIPPanel.HostedService.BackGroundService.Models.Response;

public class GetContactDetailsResponse
{
    public string LinkedId { get; set; } = string.Empty;
    public string DstChannel { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Disposition { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string PersianDate { get { return Date.ConvertDateTimeToJalaliDateTime(); } }
    public string PersianDateOnly { get { return PersianDate.Substring(0, PersianDate.IndexOf(' ')); } }
    public string PersianTimeOnly { get { return PersianDate.Substring(PersianDate.IndexOf(' ') + 1); } }
    public string Dcontext { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string Dest { get; set; } = string.Empty;
    public int? Billsecond { get; set; }
    public int? Duration { get; set; }
    public TimeSpan? DurationTimeSpan { get { return Duration != null ? TimeSpan.FromSeconds((int)Duration) : null; } }
    public string DurationStr { get { return DurationTimeSpan != null ? $"{DurationTimeSpan.Value.Hours.ToString("00")}:{DurationTimeSpan.Value.Minutes.ToString("00")}:{DurationTimeSpan.Value.Seconds.ToString("00")}" : string.Empty; } }
    public int? Waiting { get; set; }
    public TimeSpan? WaitingTimeSpan { get { return Waiting != null ? TimeSpan.FromSeconds((int)Waiting) : null; } }
    public string WaitingStr { get { return WaitingTimeSpan != null ? $"{WaitingTimeSpan.Value.Hours.ToString("00")}:{WaitingTimeSpan.Value.Minutes.ToString("00")}:{WaitingTimeSpan.Value.Seconds.ToString("00")}" : string.Empty; } }
    public string Filepath { get; set; } = string.Empty;
    public string RealFilepath { get { return !string.IsNullOrEmpty(Filepath) ? Filepath.Replace(".WAV", ".mp3").Replace(".wav", ".mp3") : Filepath; } }
    public string ShonoodFilePath { get { return $"http://10.14.8.20/recording/{Date.Year.ToString("0000")}/{Date.Month.ToString("00")}/{Date.Day.ToString("00")}/{RealFilepath}"; } }
    public string Recordingfile { get; set; } = string.Empty;
}
