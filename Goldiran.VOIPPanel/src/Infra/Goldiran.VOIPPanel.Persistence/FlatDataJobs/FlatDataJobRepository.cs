using Voip.Framework.EFCore.Common;

using Goldiran.VOIPPanel.Persistence;
using Goldiran.VOIPPanel.Domain.AggregatesModel.FlatDataJobs;
using Goldiran.VOIPPanel.Domain.AggregatesModel.FlatDataJobs.Contracts;

namespace Goldiran.VOIPPanel.Persistence.FlatDataJobs;

public class FlatDataJobRepository : BaseRepository<long, FlatDataJob>, IFlatDataJobRepository
{
    public FlatDataJobRepository(VOIPPanelContext context) : base(context)
    {
    }

}
