
using MediatR;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.IServices;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;
using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.ReadModel.Entities;

namespace Goldiran.VOIPPanel.Application.Features.Rasads.Commands.QueueRasad;

public class QueueRasadCommand : BaseCreateCommandRequest, IRequest<QueueRasadResponse>
{


    public class Handler : IRequestHandler<QueueRasadCommand, QueueRasadResponse>
    {
        private readonly IQueueRasadService _QueueRasadService;
        private readonly IReadModelContext _readModelContext;
        public Handler(IQueueRasadService QueueRasadService, IReadModelContext readModelContext)
        {
            _QueueRasadService = QueueRasadService;
            _readModelContext = readModelContext;
        }

        public async Task<QueueRasadResponse> Handle(QueueRasadCommand request, CancellationToken cancellationToken)
        {
            var userPosition=_readModelContext.Set<UserPosition>().Where(c=>c.UserId==request.UserId && c.PositionId==request.PositionId).FirstOrDefault();
            var queueCodeList=userPosition.Queues.Split(',').Select(c=>Convert.ToInt32(c)).ToList();

            var queueList=_readModelContext.Set<Queu>().Where(c=> queueCodeList.Contains(c.Code)).ToList();
            return await _QueueRasadService.GetQueueRasad(new QueueRasadRequest() { QueueRasadInfoList=queueList });

        }

    }
}

