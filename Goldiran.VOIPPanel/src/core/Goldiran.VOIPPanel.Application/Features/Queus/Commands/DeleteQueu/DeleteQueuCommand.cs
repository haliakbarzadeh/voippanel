using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus.Contracts;
using MediatR;

namespace Goldiran.VOIPPanel.Application.Features.UserPositions.Commands.DeleteUserPosition;

public class DeleteQueuCommand : BaseCreateCommandRequest, IRequest<bool>
{
    public int Id { get; set; }

    public class Handler : IRequestHandler<DeleteQueuCommand, bool>
    {
        private readonly IQueuRepository _QueuRepository;

        public Handler(IQueuRepository QueuRepository)
        {
            _QueuRepository = QueuRepository;
        }

        public async Task<bool> Handle(DeleteQueuCommand request, CancellationToken cancellationToken)
        {

            var entity = await _QueuRepository.FindByIdAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Queu), request.Id);
            }
            _QueuRepository.Delete(entity);

            await _QueuRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}




