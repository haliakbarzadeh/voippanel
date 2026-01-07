using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.Files;

    public enum OperationType
{
        [Description("پاسخگویی")]
        Answering=1,
        [Description("ناهار و نماز")]
        Launch = 2,
        [Description("استراحت")]
        Rest = 3,
        [Description("آموزش")]
        Education = 4,
        [Description("جلسه")]
        Session = 5,
        [Description("تماس مستقیم")]
        OutGoingCall = 6,
        [Description("ACWT")]
        ACWT = 7,
        [Description("قفل")]
        lOCK = 8,
        [Description("خروج")]
        Exit = 9
}

