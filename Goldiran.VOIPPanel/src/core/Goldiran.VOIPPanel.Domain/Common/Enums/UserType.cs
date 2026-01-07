using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Domain.Common.Enums;

public enum UserType
{
    [Description("سازمانی")]
    Organizational = 1,
    [Description("نماینده")]
    Agent = 2
}
