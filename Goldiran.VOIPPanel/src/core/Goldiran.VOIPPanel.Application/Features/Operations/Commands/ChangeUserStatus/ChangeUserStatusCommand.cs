using AsterNET.NetStandard.Manager.Action;
using AsterNET.NetStandard.Manager;
using AutoMapper;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Operations;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using AsterNET.NetStandard.Manager.Response;
using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.ReadModel.Entities;
using ROperation = Goldiran.VOIPPanel.ReadModel.Entities.Operation;
using Operation = Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Operation;
using Voip.Framework.Common.Exceptions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations.Enums;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.IServices;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.Application.Features.Positions.Commands.CreatePosition;

public class ChangeUserStatusCommand : BaseCreateCommandRequest, IRequest<long>
{
    public OperationType? OperationTypeId { get; set; }

    public class Handler : IRequestHandler<ChangeUserStatusCommand, long>
    {
        //IChangeUserSatusService
        private readonly IMapper _mapper;
        private readonly IOperationRepository _operationRepository;
        private readonly IUserPositionRepository _userPositionRepository;
        private readonly IReadModelContext _readModelContext;
        private readonly IQueueStatusService _queueStatusService;
        private readonly IChangeUserSatusService _changeUserSatusService;
        private int _counter = 1;

        public Handler(IMapper mapper, IOperationRepository operationRepository, IUserPositionRepository userPositionRepository, IReadModelContext readModelContext, IQueueStatusService queueStatusService, IChangeUserSatusService changeUserSatusService)
        {
            _mapper = mapper;
            _operationRepository = operationRepository;
            _userPositionRepository = userPositionRepository;
            _readModelContext = readModelContext;
            _queueStatusService = queueStatusService;
            _changeUserSatusService = changeUserSatusService;
        }

        public async Task<long> Handle(ChangeUserStatusCommand request, CancellationToken cancellationToken)
        {
            var position = await _userPositionRepository.FindByAsync(c => c.UserId == request.UserId && c.PositionId == request.PositionId && c.IsActive);
            var currentOperation = await _operationRepository.FindByAsync(c => c.UserId == request.UserId && c.PositionId == request.PositionId && c.IsCurrentStatus);
            await CheckBusinessRules(request, currentOperation, position.Queues);


            if (currentOperation != null)
            {
                //if (currentOperation.OperationTypeId == request.OperationTypeId)
                //    throw new Exception("وضعیت جدید با وضعیت فعلی بایستی متفاوت باشد");

                currentOperation.SetFinishOperation();
                _operationRepository.Update(currentOperation);
            }

            var operation = new Operation((long)request.UserId, (long)request.PositionId, (OperationType)request.OperationTypeId, position.Queues);
            _operationRepository.Add(operation);

            await ChangeStatus(position.Extension, position.Queues, (OperationType)request.OperationTypeId, currentOperation != null ? currentOperation.OperationTypeId : null);
            await _operationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return operation.Id;
        }

        private async Task ChangeStatus(string exten, string queues, OperationType operationType, OperationType? currentOperationType)
        {
            try
            {
                var queuesList = queues.Split(',').ToList();
                var queuesEntityList = _readModelContext.Set<Queu>().Where(c => queuesList.Contains(c.Code.ToString())).ToList();

                await _changeUserSatusService.ChangeUserStatus(new ChangeUserSattusRequest() { QueueList = queuesEntityList, OperationType = operationType, Exten = exten });
            }
            catch (Exception ex)
            {
                if (_counter <= 1)
                {
                    _counter++;
                    await ChangeStatus(exten, queues, operationType, currentOperationType);
                }
                else
                {
                    throw new ValidationException(new List<string>() { "مجددا امتحان کنید" });

                }

            }

        }

        private async Task CheckBusinessRules(ChangeUserStatusCommand request, Operation? currentOperation, string queues)
        {
            List<string> errors = new List<string>();
            if (currentOperation != null)
            {
                if (currentOperation.OperationTypeId == request.OperationTypeId)
                {
                    errors.Add("وضعیت جدید با وضعیت فعلی بایستی متفاوت باشد");
                    throw new ValidationException(errors);
                }
            }
            var operations = await _readModelContext.Set<ROperation>().Where(c => c.UserId == request.UserId && c.PositionId == request.PositionId && c.StartDate == DateTime.Now.Date && c.OperationTypeId == request.OperationTypeId).ToListAsync();
            var operationSetting = await _readModelContext.Set<OperationSetting>().Where(c => c.OperationTypeId == request.OperationTypeId).FirstOrDefaultAsync();
            if (operationSetting.HitLimit != null && !operations.IsNullOrEmpty() && operations.Count >= operationSetting.HitLimit)
            {
                errors.Add("به حد مجاز روزانه برای این وضعیت رسیده ایم");
            }
            
            if (operationSetting.ActiveTime != null && (operationSetting.ActiveTime.StartTime > DateTime.Now.TimeOfDay || operationSetting.ActiveTime.EndTime < DateTime.Now.TimeOfDay))
            {
                if(!(request.OperationTypeId==OperationType.Launch && DateTime.Now.Hour>=19 && DateTime.Now.Hour < 21))
                    errors.Add("در محدوده مجاز برای انجام این وضعیت قرار ندارید");
            }

            var queueList = queues.Split(',').ToList();
            //var queueLimitaions =await _readModelContext.Set<QueueLimitation>().Include(c => c.Queu).Include(c=>c.HourValues).Where(c => queueList.Contains(c.Queu.Name) && c.HourValues.Any(d=>d.HourType== (HourType)(DateTime.Now.Hour+1))).ToListAsync();
            if (request.OperationTypeId == OperationType.Rest || request.OperationTypeId == OperationType.Launch || request.OperationTypeId == OperationType.ACWT || request.OperationTypeId == OperationType.OutGoingCall)
            {
                //var restOperations = _readModelContext.Set<ROperation>().Count(c => c.IsCurrentStatus && c.StartDate == DateTime.Now.Date && c.OperationTypeId == request.OperationTypeId);
                var hourLimitaions = await _readModelContext.Set<HourValue>().Include(c => c.QueueLimitation).ThenInclude(c => c.Queue).Where(c => queueList.Contains(c.QueueLimitation.Queue.Code.ToString()) && c.HourTypeId == (HourType)(DateTime.Now.Hour + 1)).ToListAsync();

                var queueStatusList = await _queueStatusService.GetQueueStatus(new QueueStatusRequest() { QueueCodeList = queueList });

                var limitFlag = hourLimitaions.Where(c => queueStatusList.Any(d => ((int)(((d.UnPausedCount - 1) * 100) / (d.PausedCount + d.UnPausedCount))) < c.Value && c.QueueLimitation.Queue.Code.ToString() == d.QueueCode)).Any();

                if (limitFlag)
                {
                    errors.Add("امکان تغییر وضعیت به استراحت وجود ندارد");
                }
            }

            if (!errors.IsNullOrEmpty())
                throw new ValidationException(errors);

            if(currentOperation!=null && currentOperation.OperationTypeId == OperationType.Rest)
            {
                var currentOperations = await _readModelContext.Set<ROperation>().Where(c => c.UserId == request.UserId && c.PositionId == request.PositionId && c.StartDate == DateTime.Now.Date && c.OperationTypeId == currentOperation.OperationTypeId).ToListAsync();
                var currentOperationSetting = await _readModelContext.Set<OperationSetting>().Where(c => c.OperationTypeId == currentOperation.OperationTypeId).FirstOrDefaultAsync();
                if (currentOperationSetting.Duration != null && !currentOperations.IsNullOrEmpty() && (currentOperations.Sum(c => c.StatusDuration) + (DateTime.Now.TimeOfDay - currentOperation.StartTime).TotalMinutes) > currentOperationSetting.Duration)
                {
                    currentOperation.SetPenalty((int)(currentOperations.Sum(c => c.StatusDuration) + (int)(DateTime.Now.TimeOfDay - currentOperation.StartTime).TotalMinutes) - (int)currentOperationSetting.Duration);
                }
            }
           
        }


    }
}

