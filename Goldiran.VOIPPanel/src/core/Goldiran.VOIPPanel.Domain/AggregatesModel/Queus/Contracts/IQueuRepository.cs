using Voip.Framework.Domain;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.Queus.Contracts;

public interface IQueuRepository : IBaseRepository<int, Queu>
{

}
