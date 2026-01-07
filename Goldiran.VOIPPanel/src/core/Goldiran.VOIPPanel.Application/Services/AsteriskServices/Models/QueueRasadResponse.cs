using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;

public class QueueRasadResponse
{
    public List<QueueRasadResponseInfo> QueueRasadResponseInfoList { get; set; } =new List<QueueRasadResponseInfo>();
}

public class QueueRasadResponseInfo
{
    public int QueueCode {  get; set; }
    public string QueueName { get; set; }
    public int Busy {  get; set; }
    public int Free { get; set; }
    public int Break { get; set; }
    public int Waiting { get; set; }
    public int Ans { get; set; }
    public int AC { get; set; }
    public decimal SL { get; set; }
    public int? TehranRemainedCount { get; set; }
    public int? ProvinceRemainedCount { get; set; }
    public int? TotalRemainedCount { get; set; }
    public int? TotalCount { get; set; }

}
