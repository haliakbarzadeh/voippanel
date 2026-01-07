using System.ComponentModel;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Enums;
public enum PositionType
{
    [Description("کارشناس")]
    Expert = 1,

    [Description("کارشناس ارشد")]
    SeniorExpert = 2,
    [Description("سرپرست")]
    SuperVisor = 3,
    [Description("مدیر")]
    Manager = 4
}
