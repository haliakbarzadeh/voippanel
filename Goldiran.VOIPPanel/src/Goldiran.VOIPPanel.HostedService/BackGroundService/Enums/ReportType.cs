using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.HostedService.BackGroundService.Enums;

public enum ReportType
{
    [Description("نرمال")]
    Normal = 1,
    [Description("خودکار")]
    AutoDial = 2
}
