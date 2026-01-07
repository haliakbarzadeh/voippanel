using Goldiran.VOIPPanel.Domain.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Dto.Voip;

public class MasterWallboardReportDto
{
    public decimal? FCR { get; set; }
    public decimal? CSAT { get; set; }
    public decimal? SL { get; set; }
    public int? ATT { get; set; }
    public TimeSpan? ATTTimeSpan { get { return ATT != null ? TimeSpan.FromSeconds((int)ATT) : null; } }
    public string ATTStr { get { return ATTTimeSpan != null ? $"{ATTTimeSpan.Value.Minutes.ToString("00")}:{ATTTimeSpan.Value.Seconds.ToString("00")}" : string.Empty; } }
}
