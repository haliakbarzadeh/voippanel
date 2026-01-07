using Goldiran.VOIPPanel.ReadModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Entities.Asterisk;


public class SecureCall
{
    public long Id {  get; set; }
    public DateTime Date { get; set; }
    public string CustomerNumber { get; set; } = string.Empty;
    public string TechNumber { get; set; } = string.Empty;
    public string? CustomerCallStatus { get; set; }
    public string? TechCallStatus { get; set; }
    public string? UniqueId { get; set; }
    public int SessionId { get; set; } 
    public string? Duration { get; set; }
    public SecureReportType? Type { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string? ServiceNo { get; set; }
    public string? ErrorMessage { get; set; }

}
