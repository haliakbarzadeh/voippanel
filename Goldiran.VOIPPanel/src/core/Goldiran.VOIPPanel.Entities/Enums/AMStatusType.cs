using System.ComponentModel;

namespace Goldiran.VOIPPanel.ReadModel.Enums;

public enum AMStatusType
{
    [Description("منتظر")]
    Waiting = 1,
    [Description("موفق")]
    Success = 3,
    [Description("ناموفق")]
    Failed = 4,
    [Description("بی پاسخ")]
    NoAnswer = 5
}

