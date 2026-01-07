using Goldiran.VOIPPanel.Domain.AggregatesModel.TempFlatDataJobs;
using Voip.Framework.Domain;


namespace Goldiran.VOIPPanel.Domain.AggregatesModel.TempFlatDataJobs.Contracts;

public interface ITempFlatDataJobRepository : IBaseRepository<long, TempFlatDataJob>
{

}
