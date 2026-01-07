using AutoMapper;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations.Enums;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitationSettings.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using MediatR;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.Application.Features.QueueLimitations.Commands.CreateQueueLimitation;

public class CreateQueueLimitationCommand : BaseCreateCommandRequest, IRequest<int>
{
    public int QueueId { get; set; }
    public IList<AddedHourValue> HourValues { get; set; } = new List<AddedHourValue>();

    public class AddedHourValue()
    {
        public HourType HourTypeId { get; set; }
        public int Value { get; set; }
    }

    public class Handler : IRequestHandler<CreateQueueLimitationCommand, int>
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

        public async Task<int> Handle(CreateQueueLimitationCommand request, CancellationToken cancellationToken)
        {
            var existEntity = await _queueLimitationRepository.FindByAsync(c => c.QueueId == request.QueueId, new List<System.Linq.Expressions.Expression<Func<QueueLimitation, object>>>() { c => c.HourValues });
            if (existEntity != null)
            {
                if (existEntity.HourValues != null && existEntity.HourValues.Count > 0)
                {

                    var removeList = existEntity.HourValues.Where(c => !request.HourValues.Select(d => d.HourTypeId).Contains(c.HourTypeId)).ToList();
                    request.HourValues.AddRange(removeList.Select(c => new AddedHourValue() { HourTypeId = c.HourTypeId, Value = c.Value }));
                    _queueLimitationRepository.RemoveHourValues(existEntity);
                    //existEntity.Update(removeList);
                }

                existEntity.Update(request.HourValues.Select(c => new HourValue(c.HourTypeId, c.Value)).ToList());
                _queueLimitationRepository.Update(existEntity);
            }
            else
            {
                var entity = new QueueLimitation(request.QueueId, request.HourValues.Select(c => new HourValue(c.HourTypeId, c.Value)));
                _queueLimitationRepository.Add(entity);
            }

            await _queueLimitationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return 1;
        }


    }
}

