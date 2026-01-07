using AutoMapper;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.IServices;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions.Contracts;
using Goldiran.VOIPPanel.ReadModel.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore;

namespace Goldiran.VOIPPanel.Application.Features.Queus.Commands.AddUserToQueue;

public class AddUserToQueueCommand : BaseCreateCommandRequest, IRequest<bool>
{
    public string QueueName { get; set; }
    public IList<long> UserIds { get; set; } = new List<long>();

    public class Handler : IRequestHandler<AddUserToQueueCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly IUserPositionRepository _userPositionRepository;
        private readonly IOperationRepository _operationRepository;
        private readonly IReadModelContext _readModelContext;
        private readonly IChangeUserSatusService _changeUserSatusService;

        public Handler(IMapper mapper, IUserPositionRepository userPositionRepository, IReadModelContext readModelContext, IChangeUserSatusService changeUserSatusService, IOperationRepository operationRepository)
        {
            _mapper = mapper;
            _userPositionRepository = userPositionRepository;
            _readModelContext = readModelContext;
            _changeUserSatusService = changeUserSatusService;
            _operationRepository = operationRepository;
        }

        public async Task<bool> Handle(AddUserToQueueCommand request, CancellationToken cancellationToken)
        {
            string queueCode = request.QueueName.Split('_')[0];
            var userPositionIds = await _readModelContext.Set<Goldiran.VOIPPanel.ReadModel.Entities.UserPosition>().Where(c => c.IsActive && EF.Functions.Like(c.Position.ContactedParentPositionId, $"%'{request.PositionId}'%")).Select(c => c.Id).ToListAsync();

            var userPositionList = _userPositionRepository.GetAll(c => userPositionIds.Contains(c.Id)).ToList();

            var added = userPositionList.Where(c => request.UserIds.Contains(c.Id) && !c.Queues.Contains(queueCode)).ToList();

            if (added.Count > 0)
            {
                foreach (var item in added)
                {
                    if(!string.IsNullOrEmpty(item.Queues))
                        item.Update($"{item.Queues},{queueCode}", item.Extension, item.ServerIp);
                    else
                        item.Update($"{queueCode}", item.Extension, item.ServerIp);

                }

                _userPositionRepository.UpdateRange(added);
            }

            var removed = userPositionList.Where(c => !request.UserIds.Contains(c.Id) && c.Queues.Contains(queueCode)).ToList();

            await ChangeUserStatus(queueCode, removed);
            if (removed.Count > 0)
            {
                foreach (var item in removed)
                {
                    string queue = item.Queues.Replace($",{queueCode}", "").Replace($"{queueCode}", "");

                    item.Update(queue, item.Extension, item.ServerIp);
                }

                _userPositionRepository.UpdateRange(removed);
            }

            await _userPositionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        private async Task ChangeUserStatus(string queueCode, List<Domain.AggregatesModel.UserPositions.UserPosition> removedList)
        {
            var queueList = _readModelContext.Set<Queu>().AsNoTracking().Where(c => c.Code == Convert.ToInt32(queueCode)).ToList();
            foreach (var item in removedList)
            {
                if (item.Queues == queueCode)
                {
                    var currentOperation = await _operationRepository.FindByAsync(c => c.IsCurrentStatus == true && c.UserId == item.UserId && c.PositionId == item.PositionId && c.OperationTypeId != OperationType.Exit);

                    if (currentOperation != null)
                    {
                        currentOperation.SetFinishOperation();
                        var operation = new Domain.AggregatesModel.Operations.Operation(item.UserId, item.PositionId, OperationType.Exit, item.Queues);
                        _operationRepository.Add(operation);
                    }

                }
                try
                {
                    await _changeUserSatusService.ChangeUserStatus(new Services.AsteriskServices.Models.ChangeUserSattusRequest() { Exten = item.Extension, OperationType = OperationType.Exit, QueueList = queueList });

                }
                catch (Exception)
                {

                }
            }
        }

    }
}

