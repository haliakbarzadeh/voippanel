using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.Files;

    public enum CallType
{
        [Description("تماس ورودی")]
        Input=1,
        [Description("تماس اینتنرنال")]
        Internal = 2,
        [Description("تماس خروجی")]
        Output = 3,
        [Description("تماس GS")]
        GS = 4


}

