using Goldiran.VOIPPanel.HostedService.BackGroundService.Enums;

namespace Goldiran.VOIPPanel.HostedService.BackGroundService.Models.Request;

public class CreateFaltDataJobRequest
{
    public bool Status { get; set; }
    public int Count { get; set; }
    public string? Message { get; set; }
    public DateTime LastDate { get; set; }
    public ReportType ReportType { get; set; }
}
