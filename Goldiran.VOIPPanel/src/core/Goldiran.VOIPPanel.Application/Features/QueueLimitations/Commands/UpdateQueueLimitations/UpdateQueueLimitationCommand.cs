using AutoMapper;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitationSettings.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static Goldiran.VOIPPanel.Application.Features.QueueLimitations.Commands.CreateQueueLimitation.CreateQueueLimitationCommand;

namespace Goldiran.VOIPPanel.Application.Features.QueueLimitations.Commands.UpdateQueueLimitation;

public class UpdateQueueLimitationCommand : BaseCreateCommandRequest, IRequest<long>
{
    public int Id { get; set; }
    public int QueueId { get; set; }
    public IList<AddedHourValue> HourValues { get; set; } = new List<AddedHourValue>();
    public class Handler : IRequestHandler<UpdateQueueLimitationCommand,long>
    {
        private readonly IQueueLimitationRepository _queueLimitationRepository;
        private readonly IReadModelContext _readModelContext;
        private readonly IAppUserManager _appUserManager;
        private IMapper _mapper;
        public Handler(IQueueLimitationRepository queueLimitationRepository, IReadModelContext readModelContext, IAppUserManager appUserManager, IMapper mapper)
        {
            _queueLimitationRepository = queueLimitationRepository;
            _readModelContext = readModelContext;
            _appUserManager = appUserManager;
            _mapper = mapper;
        }

        public async Task<long> Handle(UpdateQueueLimitationCommand request, CancellationToken cancellationToken)
        {

            var entity = await _queueLimitationRepository.FindByIdAsync(request.Id,new List<System.Linq.Expressions.Expression<Func<QueueLimitation, object>>>() { c=>c.HourValues});
            if (entity == null)
            {
                throw new NotFoundException();
            }

            _queueLimitationRepository.RemoveHourValues(entity);
            entity.Update(request.HourValues.Select(c=>new HourValue(c.HourTypeId,c.Value)).ToList());
            _queueLimitationRepository.Update(entity);

            await _queueLimitationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }    
    }
}
