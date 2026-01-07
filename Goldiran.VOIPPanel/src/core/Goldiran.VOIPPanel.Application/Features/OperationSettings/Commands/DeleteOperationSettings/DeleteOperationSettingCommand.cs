using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettings;
using Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettingSettings.Contracts;
using MediatR;

namespace Goldiran.VOIPPanel.Application.Features.UserPositions.Commands.DeleteUserPosition;

public class DeleteOperationSettingCommand : BaseCreateCommandRequest, IRequest<bool>
{
    public int Id { get; set; }

    public class Handler : IRequestHandler<DeleteOperationSettingCommand, bool>
    {
        private readonly IOperationSettingRepository _operationSettingRepository;

        public Handler(IOperationSettingRepository operationSettingRepository)
        {
            _operationSettingRepository = operationSettingRepository;
        }

        public async Task<bool> Handle(DeleteOperationSettingCommand request, CancellationToken cancellationToken)
        {

            var entity = await _operationSettingRepository.FindByIdAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(OperationSetting), request.Id);
            }
            _operationSettingRepository.Delete(entity);

            await _operationSettingRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}




