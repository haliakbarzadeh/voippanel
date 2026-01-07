using Goldiran.VOIPPanel.Domain.AggregatesModel.SoftPhoneEvents.Enums;
using Goldiran.VOIPPanel.Domain.Common.Attributes;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.ReadModel.Dto.Voip;

public class SoftPhoneEventDto
{
    [DtoAttributes("نوع رویداد", false)]
    public SoftPhoneEventType EventType { get; set; }
    [DtoAttributes("عنوان رویداد", true)]
    public string EventTypeTitle { get; set; }

    [DtoAttributes("نام کاربر", true)]
    public string Username { get; set; }
    [DtoAttributes("داخلی", true)]
    public string Extension { get; set; }
    [DtoAttributes(nameof(Started), false)]
    public DateTime Started { get; set; }
    [DtoAttributes("زمان شروع", true)]
    public string StartedPersian
    {
        get
        {
            return Started.ConvertDateTimeToJalaliDateTime();
        }
    }

    [DtoAttributes(nameof(Finished), false)]
    public DateTime? Finished { get; set; }
    [DtoAttributes("زمان پایان", true)]
    public string FinishedPersian
    {
        get
        {
            return Finished.HasValue ? Finished.Value.ConvertDateTimeToJalaliDateTime() : string.Empty;
        }
    }

    [DtoAttributes(nameof(Duration), false)]
    public TimeSpan Duration
    {
        get
        {
            return ((Finished ?? DateTime.Now) - Started);
        }
    }

    [DtoAttributes("مدت زمان", true)]
    public string DurationStr { get { return Duration.Days > 0 ? $"{Duration.Days}:{Duration.Hours.ToString("00")}:{Duration.Minutes.ToString("00")}:{Duration.Seconds.ToString("00")}" : $"{Duration.Hours.ToString("00")}:{Duration.Minutes.ToString("00")}:{Duration.Seconds.ToString("00")}"; } }
}
