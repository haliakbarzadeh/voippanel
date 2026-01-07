using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;

public class QueueStatusRequest
{
    public List<string> QueueCodeList { get; set; } = new List<string>();
}
