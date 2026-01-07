using Voip.Framework.EFCore.Common;
using Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettings;
using Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettingSettings.Contracts;
using Goldiran.VOIPPanel.Persistence;

namespace Goldiran.VOIPPanel.Persistence.OperationSettings;

public class OperationSettingRepository : BaseRepository<int, OperationSetting>, IOperationSettingRepository
{
    public OperationSettingRepository(VOIPPanelContext context) : base(context)
    {
    }

}
