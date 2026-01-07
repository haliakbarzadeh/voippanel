using Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails.Enums;
using Goldiran.VOIPPanel.Domain.Common.Attributes;
using Voip.Framework.Domain;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails;
public class ContactDetail : AggregateRoot<long>
{
    public string LinkedId { get; private set; }
    public string DstChannel { get; private set; }
    public string Status { get; private set; }
    public string Disposition { get; private set; }
    public DateTime Date { get; private set; }
    public string Dcontext { get; private set; }
    public string? Source { get; private set; }
    public string? Dest { get; private set; }
    public int? Billsecond { get;private set; }
    public int? Duration { get;private set; }
    public int? Waiting { get;private set; }
    public string? Filepath { get;private set; }
    public string? Recordingfile { get;private set; }
    public ReportType ReportType { get; private set; }
    public string? QueueName { get; private set; }
    public int CustomWaiting { get; private set; }
    public int AgentWaiting { get; private set; }
    public int CustomToAgentWaiting { get; private set; }
    public int AgentToCustomWaiting { get; private set; }
    public string CustomerStatus { get; private set; }


    public ContactDetail(string linkedId, string dstChannel, string status, string disposition, DateTime date, string dcontext, string? source, string? dest, int? billsecond, int? duration, int? waiting, string? filepath, string? recordingfile, ReportType reportType, string? queueName, string customerStatus, int customWaiting, int agentWaiting, int customToAgentWaiting, int agentToCustomWaiting)
    {
        LinkedId = linkedId;
        DstChannel = dstChannel;
        Status = status;
        Disposition = disposition;
        Date = date;
        Dcontext = dcontext;
        Source = source;
        Dest = dest;
        Billsecond = billsecond;
        Duration = duration;
        Waiting = waiting;
        Filepath = filepath;
        Recordingfile = recordingfile;
        ReportType = reportType;
        QueueName = queueName;
        CustomerStatus = customerStatus;
        CustomWaiting = customWaiting;
        AgentWaiting = agentWaiting;
        CustomToAgentWaiting = customToAgentWaiting;
        AgentToCustomWaiting = agentToCustomWaiting;
    }

}
