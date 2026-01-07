using AsterNET.NetStandard.Manager;
using AsterNET.NetStandard.Manager.Action;
using AsterNET.NetStandard.Manager.Response;
using AutoMapper;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.IServices;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions.Contracts;
using Goldiran.VOIPPanel.ReadModel.Entities;
using MediatR;
using Operation = Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Operation;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.Services;

namespace Goldiran.VOIPPanel.Application.Features.Positions.Commands.ChangeGroupUserStatus;

public class ChangeGroupUserStatusCommand : BaseCreateCommandRequest, IRequest<bool>
{
    public string? QueueName { get; set; }
    public OperationType? OperationTypeId { get; set; }
    public IList<long> UserIdList { get; set; } = new List<long>();
    public IList<string> UserList { get; set; } = new List<string>();
    public string? ChangeReason { get; set; }
    public class Handler : IRequestHandler<ChangeGroupUserStatusCommand, bool>
    {
        private ManagerConnection _managerConnection;
        private readonly IMapper _mapper;
        private readonly IOperationRepository _operationRepository;
        private readonly IUserPositionRepository _userPositionRepository;
        private readonly IReadModelContext _readModelContext;
        private readonly IQueueStatusService _queueStatusService;
        private readonly IUserPositionQueryService _userPositionQueryService;
        private readonly IChangeUserSatusService _changeUserSatusService;
        public Handler(IMapper mapper, IOperationRepository operationRepository, IUserPositionRepository userPositionRepository, IUserPositionQueryService userPositionQueryService, IReadModelContext readModelContext, IQueueStatusService queueStatusService, IChangeUserSatusService changeUserSatusService)
        {
            _mapper = mapper;
            _operationRepository = operationRepository;
            _userPositionRepository = userPositionRepository;
            _userPositionQueryService = userPositionQueryService;
            _readModelContext = readModelContext;
            _queueStatusService = queueStatusService;
            _changeUserSatusService = changeUserSatusService;
        }

        public async Task<bool> Handle(ChangeGroupUserStatusCommand request, CancellationToken cancellationToken)
        {

            //_managerConnection = new ManagerConnection("10.121.21.26", 5038, "hamid", "1234");

            //_managerConnection.Login();

            //var userPosition = await _userPositionQueryService.GetUserPositionById((long)request.PositionId);

            for (int i = 0; i < request.UserIdList.Count; i++)
            {
                var currentOperation = await _operationRepository.FindByAsync(c => c.UserId == request.UserIdList[i] && c.IsCurrentStatus && c.OperationTypeId!=OperationType.Exit);
                if (currentOperation == null)
                {
                    continue;
                }
                if (!(CheckBusinessRules(request, currentOperation)))
                    continue;

                var position = _userPositionRepository.GetAll(c => c.UserId == request.UserIdList[i] && c.IsActive).FirstOrDefault();

                if (!(await ChangeStatus(request.UserList[i], position.Queues, (OperationType)request.OperationTypeId, currentOperation != null ? currentOperation.OperationTypeId : null)))
                    continue;

                currentOperation.SetFinishOperation();
                _operationRepository.Update(currentOperation);

                var operation = new Operation((long)request.UserIdList[i],currentOperation.PositionId, (OperationType)request.OperationTypeId, position.Queues, (long)request.UserId, request.ChangeReason);
                _operationRepository.Add(operation);

            }

            //_managerConnection.Logoff();

            await _operationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        private async Task<bool> ChangeStatus(string exten, string queues, OperationType operationType, OperationType? currentOperationType)
        {
            try
            {

                var queuesList = queues.Split(',').ToList();
                var queuesEntityList = _readModelContext.Set<Queu>().Where(c => queuesList.Contains(c.Code.ToString())).ToList();

                await _changeUserSatusService.ChangeUserStatus(new ChangeUserSattusRequest() { QueueList = queuesEntityList, OperationType = operationType, Exten = exten });
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;

        }

        private bool CheckBusinessRules(ChangeGroupUserStatusCommand request, Operation? currentOperation)
        {
            if (currentOperation != null && currentOperation.OperationTypeId == request.OperationTypeId)
            {
                return false;
            }

            return true;
        }
    }
}

