using Voip.Framework.EFCore.Common;

using Goldiran.VOIPPanel.Persistence;
using Goldiran.VOIPPanel.Domain.AggregatesModel.TempFlatDataJobs;
using Goldiran.VOIPPanel.Domain.AggregatesModel.TempFlatDataJobs.Contracts;

namespace Goldiran.VOIPPanel.Persistence.TempFlatDataJobs;

public class TempFlatDataJobRepository : BaseRepository<long, TempFlatDataJob>, ITempFlatDataJobRepository
{
    public TempFlatDataJobRepository(VOIPPanelContext context) : base(context)
    {
    }

}
