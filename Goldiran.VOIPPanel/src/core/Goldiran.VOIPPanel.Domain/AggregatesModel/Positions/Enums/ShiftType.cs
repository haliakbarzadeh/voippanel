using System.ComponentModel;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Enums;
public enum ShiftType
{
    [Description("شیفت روزانه")]
    Daily = 1,
    [Description("شیفت صبح")]
    Morning = 2,
    [Description("شیفت عصر")]
    Night = 3
}
