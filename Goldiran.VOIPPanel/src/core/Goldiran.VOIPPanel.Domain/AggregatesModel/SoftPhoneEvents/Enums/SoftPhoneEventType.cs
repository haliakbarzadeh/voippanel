using System.ComponentModel;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.SoftPhoneEvents.Enums
{
    public enum SoftPhoneEventType
    {
        [Description("DND")]
        Dnd = 1,

        [Description("رجیستری")]
        UnRegistered = 2,
    }
}
