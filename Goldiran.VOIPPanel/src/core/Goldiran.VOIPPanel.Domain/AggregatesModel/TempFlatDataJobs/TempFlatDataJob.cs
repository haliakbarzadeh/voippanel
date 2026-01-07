using Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails.Enums;
using Voip.Framework.Domain;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.TempFlatDataJobs;
public class TempFlatDataJob : AggregateRoot<long>
{
    public bool Status { get; private set; }
    public int Count { get; private set; }
    public string? Message { get; private set; }
    public DateTime LastDate { get; private set; }
    public ReportType ReportType { get; private set; }


    public TempFlatDataJob(bool status, int count, string? message, DateTime lastDate,ReportType reportType)
    {
        Status = status;
        Count = count;
        Message = message;
        LastDate = lastDate;
        ReportType = reportType;
    }

}
