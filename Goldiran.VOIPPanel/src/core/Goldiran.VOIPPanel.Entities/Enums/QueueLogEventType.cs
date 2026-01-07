using Goldiran.VOIPPanel.ReadModel.Dto.Voip;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using System.ComponentModel;

namespace Goldiran.VOIPPanel.ReadModel.Enums;

public enum QueueLogEventType
{
    [Description("ABANDON")]
    ABANDON = 1,
    [Description("BLINDTRANSFER")]
    BLINDTRANSFER = 2,
    [Description("COMPLETEAGENT")]
    COMPLETEAGENT = 3,
    [Description("COMPLETECALLER")]
    COMPLETECALLER = 4,
    [Description("EXITWITHTIMEOUT")]
    EXITWITHTIMEOUT = 5,
    [Description("EXITWITHKEY")]
    EXITWITHKEY = 6,
    [Description("RINGCANCELED")]
    RINGCANCELED = 7,
    [Description("RINGNOANSWER")]
    RINGNOANSWER = 8
}

