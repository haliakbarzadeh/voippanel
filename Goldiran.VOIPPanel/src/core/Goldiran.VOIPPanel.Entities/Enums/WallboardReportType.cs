using Goldiran.VOIPPanel.ReadModel.Dto.Voip;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using System.ComponentModel;

namespace Goldiran.VOIPPanel.ReadModel.Enums;

public enum WallboardReportType
{

    [Description("تعداد تماس")]
    AgentContacts = 1,
    [Description("میانگین زمان مکالمه")]
    AverageTimeContact = 2,
    [Description("مجموع زمان مکالمه")]
    SumTimeContact = 3,
    [Description("CSAT")]
    CSAT = 4,
    [Description("زمان استراحت")]
    RestOperation = 5,
    [Description("زمان پاسخگویی")]
    AnsweringOperation = 6
}

