using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitationSettings.Contracts;
using MediatR;

namespace Goldiran.VOIPPanel.Application.Features.UserPositions.Commands.DeleteUserPosition;

public class DeleteQueueLimitationCommand : BaseCreateCommandRequest, IRequest<bool>
{
    public int Id { get; set; }

    public class Handler : IRequestHandler<DeleteQueueLimitationCommand, bool>
    {
        private readonly IQueueLimitationRepository _queueLimitationRepository;

        public Handler(IQueueLimitationRepository queueLimitationRepository)
        {
            _queueLimitationRepository = queueLimitationRepository;
        }

        public async Task<bool> Handle(DeleteQueueLimitationCommand request, CancellationToken cancellationToken)
        {

            var entity = await _queueLimitationRepository.FindByIdAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(QueueLimitation), request.Id);
            }
            _queueLimitationRepository.Delete(entity);

            await _queueLimitationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}




