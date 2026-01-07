using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Entities.Asterisk;


public class AskCustomer
{
    public long Id {  get; set; }
    public DateTime Date { get; set; }
    public int Agent { get; set; }
    public TimeSpan Time {  get; set; }
    public string CallerId { get; set; } = string.Empty;
    public string Queue { get; set; } = string.Empty;
    public string Response {  get; set; } = string.Empty;   
    public string UniqueId { get; set; } = string.Empty;

}
