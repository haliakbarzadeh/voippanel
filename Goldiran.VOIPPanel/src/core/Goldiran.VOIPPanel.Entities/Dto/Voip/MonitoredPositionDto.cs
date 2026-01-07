using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Dto;

public class MonitoredPositionDto
{
    public long SourcePositionId { get; set; }
    public string SourcePositionTitle { get; set; }
    public long DestPositionId { get;  set; }
    public string DestPositionTitle { get; set; }
}
