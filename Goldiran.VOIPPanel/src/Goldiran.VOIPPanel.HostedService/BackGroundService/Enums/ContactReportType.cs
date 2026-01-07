using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.HostedService.BackGroundService.Enums;

public enum ContactReportType
{
        [Description("جزئیات تماس")]
        Detail=1,
        [Description("تماس خودکار")]
        AutoDial = 2
}

