using Voip.Framework.Domain;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Contracts;

public interface IOperationRepository : IBaseRepository<long, Operation>
{

}
