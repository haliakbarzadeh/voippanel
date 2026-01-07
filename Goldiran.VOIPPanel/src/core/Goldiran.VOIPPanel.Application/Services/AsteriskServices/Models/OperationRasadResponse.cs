using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;

public class OperationRasadResponse
{
    public string QueueCode {  get; set; }
    public string Caller { get; set; }
    public string Extension { get; set; }
    public string Duration { get; set; }
}
