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

namespace Goldiran.VOIPPanel.Application.Features.QueueLimitations.Commands.UpdateHourValue;

public class UpdateHourValueCommand : BaseCreateCommandRequest, IRequest<long>
{
    public int Id { get; set; }
    public int value { get; set; }
    public class Handler : IRequestHandler<UpdateHourValueCommand,long>
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

        public async Task<long> Handle(UpdateHourValueCommand request, CancellationToken cancellationToken)
        {

            var entity = await _queueLimitationRepository.GetAll(c=>c.HourValues.Any(c=>c.Id==request.Id), new List<System.Linq.Expressions.Expression<Func<QueueLimitation, object>>>() { c => c.HourValues }).FirstOrDefaultAsync();
            if (entity == null)
            {
                throw new NotFoundException();
            }
            entity.UpdateHourValue(request.Id,request.value);
            _queueLimitationRepository.Update(entity);
            await _queueLimitationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }    
    }
}
