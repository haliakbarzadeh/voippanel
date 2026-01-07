using AutoMapper;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.Domain.AggregatesModel.MonitoredPositions.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Application.Features.UserPositions.Commands.DeleteUserPosition;

public class DeleteMonitoredPositionCommand : BaseCreateCommandRequest, IRequest<bool>
{
    public long SourcePositionId { get; set; }

    public class Handler : IRequestHandler<DeleteMonitoredPositionCommand,bool>
    {
        private readonly IMonitoredPositionRepository _monitoredPositionRepository;

        public Handler(IMonitoredPositionRepository monitoredPositionRepository)
        {
            _monitoredPositionRepository = monitoredPositionRepository;
        }

        public async Task<bool> Handle(DeleteMonitoredPositionCommand request, CancellationToken cancellationToken)
        {

            var entityList = _monitoredPositionRepository.GetAll(c=>c.SourcePositionId==request.SourcePositionId);
            if (entityList == null)
            {
                return true;
            }
            _monitoredPositionRepository.Delete(entityList.ToList());

            await _monitoredPositionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }     
    }
}



