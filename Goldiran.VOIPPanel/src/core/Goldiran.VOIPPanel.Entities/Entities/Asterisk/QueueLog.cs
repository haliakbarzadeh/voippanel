using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Entities.Asterisk;


public class QueueLog
{
    public long Id {  get; set; }
    public DateTime Created { get; set; }
    public string Agent { get; set; }=string.Empty;
    public string CallId { get; set; } = string.Empty;
    public string Data1 { get; set; } = string.Empty;
    public string Data2{ get; set; } = string.Empty;
    public string Data3 { get; set; } = string.Empty;
    public string Data4 { get; set; } = string.Empty;
    public string Data5 { get; set; } = string.Empty;
    public string Event { get; set; } = string.Empty;
    public string QueueNumber { get; set; } = string.Empty;
    public string Time { get; set; } = string.Empty;

}
