using System.ComponentModel;

namespace Goldiran.VOIPPanel.ReadModel.Enums;

public enum SecureReportType
{
    [Description("تکنسین به مشتری")]
    TechToCust = 1,
    [Description("تکنسین به مشتری")]
    CustToTech = 2,
    [Description("مشتری به مرکز اطلاعات")]
    CustToInfo = 3
}

