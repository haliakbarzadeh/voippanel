using Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Dto;

public class TempFlatDataJobDto
{
    public bool Status { get; set; }
    public int Count { get; set; }
    public string? Message { get; set; }
    public DateTime LastDate { get; set; }
    public ReportType ReportType { get;  set; }

}
