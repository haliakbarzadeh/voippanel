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
using Voip.Framework.Common.Exceptions;
using static ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.Services;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.Application.Features.Positions.Commands.CreatePosition;

public class ChangeUserStatusBySupervisorCommand : BaseCreateCommandRequest, IRequest<long>
{
    public OperationType? OperationTypeId { get; set; }
    public long? User { get; set; }


    public class Handler : IRequestHandler<ChangeUserStatusBySupervisorCommand, long>
    {
        private readonly IMapper _mapper;
        private readonly IOperationRepository _operationRepository;
        private readonly IUserPositionRepository _userPositionRepository;
        private readonly IReadModelContext _readModelContext;
        private readonly IQueueStatusService _queueStatusService;
        private readonly IChangeUserSatusService _changeUserSatusService;

        public Handler(IMapper mapper, IOperationRepository operationRepository, IUserPositionRepository userPositionRepository, IReadModelContext readModelContext, IQueueStatusService queueStatusService, IChangeUserSatusService changeUserSatusService)
        {
            _mapper = mapper;
            _operationRepository = operationRepository;
            _userPositionRepository = userPositionRepository;
            _readModelContext = readModelContext;
            _queueStatusService = queueStatusService;
            _changeUserSatusService = changeUserSatusService;
        }

        public async Task<long> Handle(ChangeUserStatusBySupervisorCommand request, CancellationToken cancellationToken)
        {
            var positionList =  _userPositionRepository.GetAll(c => c.UserId == request.User && c.IsActive).ToList();
            foreach (var position in positionList)
            {
                var currentOperation = await _operationRepository.FindByAsync(c => c.UserId == position.UserId && c.PositionId==position.PositionId && c.IsCurrentStatus && c.OperationTypeId!=OperationType.Exit);
                if(currentOperation==null)
                    continue;

                await CheckBusinessRules(request, currentOperation, position.Queues);

                if (currentOperation != null)
                {
                    //if (currentOperation.OperationTypeId == request.OperationTypeId)
                    //    throw new Exception("وضعیت جدید با وضعیت فعلی بایستی متفاوت باشد");

                    currentOperation.SetFinishOperation();
                    _operationRepository.Update(currentOperation);
                }

                var operation = new Operation((long)request.User,currentOperation.PositionId, (OperationType)request.OperationTypeId, position.Queues);
                _operationRepository.Add(operation);

                await ChangeStatus(position.Extension, position.Queues, (OperationType)request.OperationTypeId, currentOperation != null ? currentOperation.OperationTypeId : null);
                await _operationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }

            return 1;
        }

        private async Task ChangeStatus(string exten, string queues, OperationType operationType, OperationType? currentOperationType)
        {

            var queuesList = queues.Split(',').ToList();
            var queuesEntityList = _readModelContext.Set<Queu>().Where(c => queuesList.Contains(c.Code.ToString())).ToList();

            await _changeUserSatusService.ChangeUserStatus(new ChangeUserSattusRequest() { QueueList = queuesEntityList, OperationType = operationType, Exten = exten });
        }

        private async Task CheckBusinessRules(ChangeUserStatusBySupervisorCommand request, Operation? currentOperation,string queues)
        {
            List<string> errors = new List<string>();
            if (currentOperation != null)
            {
                if (currentOperation.OperationTypeId == OperationType.Exit)
                {
                    errors.Add("امکان تغییر وضعیت از خروج وجود ندارد");
                    throw new ValidationException(errors);
                }
            }

            if (currentOperation != null)
            {
                if (currentOperation.OperationTypeId == request.OperationTypeId)
                {
                    errors.Add("وضعیت جدید با وضعیت فعلی بایستی متفاوت باشد");
                    throw new ValidationException(errors);
                }
            }
            
            var operations = await _readModelContext.Set<ROperation>().Where(c =>c.UserId==request.UserId && c.StartDate == DateTime.Now.Date && c.OperationTypeId == request.OperationTypeId).ToListAsync();
            var operationSetting = await _readModelContext.Set<OperationSetting>().Where(c => c.OperationTypeId == request.OperationTypeId).FirstOrDefaultAsync();
            if (operationSetting.HitLimit != null && !operations.IsNullOrEmpty() && operations.Count >= operationSetting.HitLimit)
            {
                errors.Add("به حد مجاز روزانه برای این وضعیت رسیده ایم");
            }
            if (operationSetting.ActiveTime != null && (operationSetting.ActiveTime.StartTime > DateTime.Now.TimeOfDay || operationSetting.ActiveTime.EndTime < DateTime.Now.TimeOfDay))
            {
                errors.Add("در محدوده مجاز برای انجام این وضعیت قرار ندارید");
            }

            var queueList = queues.Split(',').ToList();
            //var queueLimitaions =await _readModelContext.Set<QueueLimitation>().Include(c => c.Queu).Include(c=>c.HourValues).Where(c => queueList.Contains(c.Queu.Name) && c.HourValues.Any(d=>d.HourType== (HourType)(DateTime.Now.Hour+1))).ToListAsync();
            if (request.OperationTypeId == OperationType.Rest)
            {
                //var restOperations = _readModelContext.Set<ROperation>().Count(c => c.IsCurrentStatus && c.StartDate == DateTime.Now.Date && c.OperationTypeId == request.OperationTypeId);
                var hourLimitaions = await _readModelContext.Set<HourValue>().Include(c => c.QueueLimitation).ThenInclude(c => c.Queue).Where(c => queueList.Contains(c.QueueLimitation.Queue.Code.ToString()) && c.HourTypeId == (HourType)(DateTime.Now.Hour + 1) ).ToListAsync();

                var queueStatusList = await _queueStatusService.GetQueueStatus(new QueueStatusRequest() { QueueCodeList = queueList });

                var limitFlag = hourLimitaions.Where(c => queueStatusList.Any(d => ((int)((d.UnPausedCount * 100) / (d.PausedCount + d.UnPausedCount))) < c.Value && c.QueueLimitation.Queue.Code.ToString() == d.QueueCode)).Any();

                if (limitFlag)
                {
                    errors.Add("امکان تغییر وضعیت به استراحت وجود ندارد");
                }
            }

            if (!errors.IsNullOrEmpty())
                throw new ValidationException(errors);


            if (currentOperation != null && currentOperation.OperationTypeId==OperationType.Rest)
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

