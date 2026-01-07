using Goldiran.VOIPPanel.Domain.AggregatesModel.SoftPhoneEvents;
using Goldiran.VOIPPanel.Domain.AggregatesModel.SoftPhoneEvents.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.SoftPhoneEvents.Enums;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions.Contracts;
using MediatR;
using Voip.Framework.Domain.Models.CQRS;

namespace Goldiran.VOIPPanel.Application.Features.SoftPhoneEvents.Commands.CreateSoftPhoneEvent
{
    public class CreateSoftPhoneEventCommand : BaseCreateCommandRequest, IRequest<bool>
    {
        public string Extension { get; set; } = string.Empty;
        public SoftPhoneEventType SoftPhoneEventType { get; set; }


        public class Handler : IRequestHandler<CreateSoftPhoneEventCommand, bool>
        {
            private readonly ISoftPhoneEventsRepository _softPhoneEventsRepository;
            private readonly IUserPositionRepository _userPositionRepository;

            public Handler(ISoftPhoneEventsRepository softPhoneEventsRepository, IUserPositionRepository userPositionRepository)
            {
                _softPhoneEventsRepository = softPhoneEventsRepository;
                _userPositionRepository = userPositionRepository;
            }

            public async Task<bool> Handle(CreateSoftPhoneEventCommand request, CancellationToken cancellationToken)
            {
                var userPosition = await _userPositionRepository.FindByAsync(x => x.Extension == request.Extension && x.IsActive);

                if (userPosition == null)
                    return false;

                var @event = await _softPhoneEventsRepository.FindByAsync(x => x.UserId == userPosition!.UserId && x.UserPositionId == userPosition!.Id && x.FinishedAt == null && x.EventType == request.SoftPhoneEventType);

                if (@event == null)
                {
                    @event = new SoftPhoneEvent(request.SoftPhoneEventType, userPosition.Id, userPosition.UserId);
                    _softPhoneEventsRepository.Add(@event);

                }
                else
                {
                    @event.Finished();
                    _softPhoneEventsRepository.Update(@event);
                }


                await _softPhoneEventsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return true;
            }
        }
    }
}
