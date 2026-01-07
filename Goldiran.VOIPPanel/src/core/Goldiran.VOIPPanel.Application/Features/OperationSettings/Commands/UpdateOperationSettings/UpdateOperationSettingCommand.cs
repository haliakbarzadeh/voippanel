using AutoMapper;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettings.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettings.ValueObjects;
using Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettingSettings.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static Goldiran.VOIPPanel.Application.Features.OperationSettings.Commands.CreateOperationSetting.CreateOperationSettingCommand;

namespace Goldiran.VOIPPanel.Application.Features.OperationSettings.Commands.UpdateOperationSetting;

public class UpdateOperationSettingCommand : BaseCreateCommandRequest, IRequest<long>
{
    public int Id { get; set; }
    public OperationType? OperationTypeId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public string Icon { get; set; }
    public string Label { get; set; }
    public int? Order { get; set; }
    public bool InQueue { get; set; }
    public bool IsActive { get; set; }
    public bool Pasuse { get; set; }
    public bool ShowToUser { get; set; }
    public int? HitLimit { get; set; }
    public int? Duration { get; set; }
    public int? Penalty { get; set; }
    public ActTime? ActiveTime { get; set; }
    public class Handler : IRequestHandler<UpdateOperationSettingCommand,long>
    {
        private readonly IOperationSettingRepository _operationSettingRepository;
        private readonly IReadModelContext _readModelContext;
        private readonly IAppUserManager _appUserManager;
        private IMapper _mapper;
        public Handler(IOperationSettingRepository operationSettingRepository, IReadModelContext readModelContext, IAppUserManager appUserManager, IMapper mapper)
        {
            _operationSettingRepository = operationSettingRepository;
            _readModelContext = readModelContext;
            _appUserManager = appUserManager;
            _mapper = mapper;
        }

        public async Task<long> Handle(UpdateOperationSettingCommand request, CancellationToken cancellationToken)
        {

            var entity = await _operationSettingRepository.FindByIdAsync(request.Id);
            if (entity == null)
            {
                throw new NotFoundException();
            }

            entity.Update((OperationType)request.OperationTypeId, request.Name, request.Description, request.Color, request.Icon, request.Label, (int)request.Order, request.InQueue, request.IsActive, request.Pasuse, request.ShowToUser, request.HitLimit, request.Duration, request.Penalty);

            if (request.ActiveTime != null)
                entity.SetActiveTime(new Domain.AggregatesModel.OperationSettings.ValueObjects.ActiveTime(TimeSpan.Parse(request.ActiveTime.StartTime), TimeSpan.Parse(request.ActiveTime.EndTime)));

            _operationSettingRepository.Update(entity);

            await _operationSettingRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }    
    }
}
