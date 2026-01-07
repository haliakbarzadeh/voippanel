using Goldiran.VOIPPanel.HostedService.BackGroundService.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Goldiran.VOIPPanel.HostedService.BackGroundService.Models.Request;

public class GetContactDetailsRequest
{
    public bool IsRestricted { get; set; } = true;
    public ContactReportType ContactReportType { get; set; } = ContactReportType.Detail;
    public bool IsJob { get; set; } = false;
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public TimeSpan? FromTime { get; set; }
    public TimeSpan? ToTime { get; set; }
    public string? TypeCalls { get; set; }
    public string? Agents { get; set; }
    public string? Phone { get; set; }
    public int? OrderBy { get; set; }
}
