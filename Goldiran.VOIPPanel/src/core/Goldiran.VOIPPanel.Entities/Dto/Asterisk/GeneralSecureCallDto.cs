using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Enums;
using Goldiran.VOIPPanel.ReadModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;

public class GeneralSecureCallDto
{
    public long CustToTech {  get; set; }
    public long SucCustToTech { get; set; }
    public long TechToCust { get; set; }
    public long SucTechToCust { get; set; }
    public long CustToInfo { get; set; }
    public long SucCustToInfo { get; set; }


}
