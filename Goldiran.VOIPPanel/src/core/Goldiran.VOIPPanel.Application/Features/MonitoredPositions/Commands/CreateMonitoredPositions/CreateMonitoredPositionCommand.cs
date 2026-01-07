using AutoMapper;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.Domain.AggregatesModel.MonitoredPositions.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.MonitoredPositions;
using MediatR;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Voip.Framework.Common.Exceptions;

namespace Goldiran.VOIPPanel.Application.Features.MonitoredPositions.Commands.CreateMonitoredPosition;

public class CreateMonitoredPositionCommand : BaseCreateCommandRequest, IRequest<bool>
{
    public long SourcePositionId { get; set; }
    public List<long> DestPositionIdList { get; set; } = new List<long>();

    public class Handler : IRequestHandler<CreateMonitoredPositionCommand, bool>
    {
        private readonly IMonitoredPositionRepository _MonitoredPositionRepository;
        private readonly IReadModelContext _readModelContext;
        private readonly IAppUserManager _appUserManager;
        private IMapper _mapper;
        public Handler(IMonitoredPositionRepository MonitoredPositionRepository, IReadModelContext readModelContext, IAppUserManager appUserManager, IMapper mapper)
        {
            _MonitoredPositionRepository = MonitoredPositionRepository;
            _readModelContext = readModelContext;
            _appUserManager = appUserManager;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateMonitoredPositionCommand request, CancellationToken cancellationToken)
        {
            var enities=_MonitoredPositionRepository.GetAll(c=>c.SourcePositionId==request.SourcePositionId).ToList();

            if (enities.Any())
            {
                _MonitoredPositionRepository.Delete(enities);
            }
            var addedList=request.DestPositionIdList.Select(c=>new MonitoredPosition(request.SourcePositionId,c)).ToList();

            _MonitoredPositionRepository.AddRange(addedList);

            await _MonitoredPositionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
           
            return true;
        }

    }
}

