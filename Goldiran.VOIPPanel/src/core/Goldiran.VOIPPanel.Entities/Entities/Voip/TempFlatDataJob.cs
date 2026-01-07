using Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails.Enums;
using Voip.Framework.Domain;

namespace Goldiran.VOIPPanel.ReadModel.Entities;
public class TempFlatDataJob : BaseQueryEntity<long>
{
    public bool Status { get; set; }
    public int Count { get; set; }
    public string? Message { get; set; }
    public DateTime LastDate { get; set; }
    public ReportType ReportType { get; set; }

}
