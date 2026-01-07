using Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails.Enums;
using Voip.Framework.Domain;

namespace Goldiran.VOIPPanel.ReadModel.Entities;
public class ContactDetail : BaseQueryEntity<long>
{
    public string LinkedId { get; set; }=string.Empty;
    public string DstChannel { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Disposition { get; set; }=string.Empty;
    public DateTime Date { get; set; }
    public string Dcontext { get; set; } = string.Empty;
    public string? Source { get; set; }
    public string? Dest { get; set; }
    public int? Billsecond { get; set; }
    public int? Duration { get; set; }
    public int? Waiting { get; set; }
    public string? Filepath { get; set; }
    public string? Recordingfile { get; set; }
    public ReportType ReportType { get; set; }
    public string? QueueName { get; set; }
    public int CustomWaiting { get;  set; }
    public int AgentWaiting { get;  set; }
    public int CustomToAgentWaiting { get;  set; }
    public int AgentToCustomWaiting { get;  set; }
    public string CustomerStatus { get; set; } = string.Empty;


}
