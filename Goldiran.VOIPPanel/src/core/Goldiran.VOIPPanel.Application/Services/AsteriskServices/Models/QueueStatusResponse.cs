using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;

public class QueueStatusResponse
{
    public string QueueCode {  get; set; }
    public int PausedCount { get; set; }
    public int UnPausedCount { get; set; }
    public IList<string> ExtensionList { get; set; }=new List<string>();
}
