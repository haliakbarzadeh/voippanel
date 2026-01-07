using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.Files;

    public enum OperationReportType
{
        [Description("یک هفته")]
        OneWeek=1,
        [Description("دو هفته")]
        TwoWeek = 2,
        [Description("یک ماه")]
        OneMounth = 3,
        [Description("یک فصل")]
        OneSeason = 4,
        [Description("یک سال")]
        OneYear = 5
}

