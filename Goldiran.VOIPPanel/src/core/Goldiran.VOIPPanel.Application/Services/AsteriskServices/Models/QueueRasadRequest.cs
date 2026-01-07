using Goldiran.VOIPPanel.ReadModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;

public class QueueRasadRequest
{
    public List<Queu> QueueRasadInfoList { get; set; }=new List<Queu>();
}

//public class QueueRasadInfo 
//{
//    public int QueueCode {  get; set; }
//    public string QueueName { get; set; }
//}
