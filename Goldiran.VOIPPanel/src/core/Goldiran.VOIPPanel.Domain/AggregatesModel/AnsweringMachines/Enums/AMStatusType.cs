using System.ComponentModel;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.AnsweringMachines.Enums;

public enum AMStatusType
{
    [Description("منتظر")]
    Waiting = 1,
    [Description("عدم تماس")]
    NoContact = 2,
    [Description("موفق")]
    Success = 3,
    [Description("ناموفق")]
    Failed = 4,
    [Description("بی پاسخ")]
    NoAnswer = 5,
    [Description("تماس گرفته شده")]
    Contacted = 6
}

