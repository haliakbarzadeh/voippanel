using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Enums;
using Goldiran.VOIPPanel.Domain.Common.Attributes;
using Goldiran.VOIPPanel.ReadModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;

public class SecureCallDto
{
    [DtoAttributes("شناسه", false)]
    public long Id {  get; set; }
    [DtoAttributes("شناسه", false)]
    public DateTime Date { get; set; }
    [DtoAttributes("تاریخ و زمان", true)]
    public string PersianDate { get { return Date.ConvertDateTimeToJalaliDateTime(); } }
    [DtoAttributes("شماره تماس مشتری", true)]
    public string CustomerNumber { get; set; } = string.Empty;
    [DtoAttributes("شماره تکنسین", true)]
    public string TechNumber { get; set; } = string.Empty;
    [DtoAttributes("وضعیت تماس مشتری", true)]
    public string? CustomerCallStatus { get; set; }
    [DtoAttributes("وضعیت تماس تکنسین", true)]
    public string? TechCallStatus { get; set; }
    [DtoAttributes("شناسه", false)]
    public string? UniqueId { get; set; }
    [DtoAttributes("شناسه", false)]
    public int SessionId { get; set; }
    [DtoAttributes("مدت مکالمه", true)]
    public string? Duration { get; set; }
    public SecureReportType? Type { get; set; }
    [DtoAttributes("جهت تماس", true)]
    public string TypeStr { get { return Type != null ? Type.Description() : string.Empty; } }
    [DtoAttributes("شناسه", false)]
    public string FileName { get; set; } = string.Empty;
    [DtoAttributes("شناسه", false)]
    public string? ServiceNo { get; set; }
    [DtoAttributes("شناسه", false)]
    public string? ErrorMessage { get; set; }
    [DtoAttributes("شناسه", false)]
    public string RealFilepath { get { return !string.IsNullOrEmpty(FileName) ? FileName.Replace(".WAV", ".mp3").Replace(".wav", ".mp3").Replace(".gsm", ".mp3") : FileName; } }
    [DtoAttributes("شناسه", false)]
    public string ShonoodFilePath { get { return $"http://10.14.8.20/recording/{Date.Year.ToString("0000")}/{Date.Month.ToString("00")}/{Date.Day.ToString("00")}/{RealFilepath}"; } }
}
