using Voip.Framework.Domain;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus;
using Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.Queus.Contracts;

public interface IContactDetailRepository : IBaseRepository<long, ContactDetail>
{

}
