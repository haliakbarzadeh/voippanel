using Voip.Framework.EFCore.Common;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitationSettings.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus.Contracts;
using Goldiran.VOIPPanel.Persistence;
using Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails;

namespace Goldiran.VOIPPanel.Persistence.ContactDetails;

public class ContactDetailRepository : BaseRepository<long, ContactDetail>, IContactDetailRepository
{
    public ContactDetailRepository(VOIPPanelContext context) : base(context)
    {
    }

}
