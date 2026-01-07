
using MediatR;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.IServices;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;
using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.ReadModel.Entities;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.Services;
using Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Enums;

namespace Goldiran.VOIPPanel.Application.Features.Rasads.Commands.MarkettingRasad;

public class MarkettingRasadCommand : BaseCreateCommandRequest, IRequest<QueueRasadResponse>
{


    public class Handler : IRequestHandler<MarkettingRasadCommand, QueueRasadResponse>
    {
        private readonly IQueueRasadService _queueRasadService;
        private readonly IReadModelContext _readModelContext;
        private readonly IRemainedCallQueryService _remainedCallQueryService;
        public Handler(IQueueRasadService queueRasadService, IRemainedCallQueryService remainedCallQueryService, IReadModelContext readModelContext)
        {
            _queueRasadService = queueRasadService;
            _remainedCallQueryService = remainedCallQueryService;
            _readModelContext = readModelContext;
        }

        public async Task<QueueRasadResponse> Handle(MarkettingRasadCommand request, CancellationToken cancellationToken)
        {
            var remainedCallDto = new RemainedCallDto();
            var fromDate = DateTime.Now.Date.Add(new TimeSpan(0, 0, 0));
            var toDate = DateTime.Now.Date.Add(new TimeSpan(23, 59, 59));


            //var userPosition = _readModelContext.Set<UserPosition>().Where(c => c.UserId == request.UserId && c.PositionId == request.PositionId).FirstOrDefault();
            //var queueCodeList = userPosition.Queues.Split(',').Select(c => Convert.ToInt32(c)).ToList();
            var queueCodeList = new List<int>() { 22, 25 };

            var queueList=_readModelContext.Set<Queu>().Where(c=> queueCodeList.Contains(c.Code)).ToList();
            var rasadResult= await _queueRasadService.GetQueueRasad(new QueueRasadRequest() { QueueRasadInfoList = queueList });

            foreach (var rasad in rasadResult.QueueRasadResponseInfoList)
            {
                if (rasad.QueueCode == 22)
                {
                    remainedCallDto = await _remainedCallQueryService.GetRemainedCall(new GetRemainedCallQuery() { FromDate = fromDate, ToDate = toDate, RemainedCallType=RemainedCallType.Marketting});

                    rasad.TehranRemainedCount = remainedCallDto.TehranRemainedCount;
                    rasad.ProvinceRemainedCount=remainedCallDto.ProvinceRemainedCount;
                    rasad.TotalRemainedCount=remainedCallDto.TotalRemainedCount;
                    rasad.TotalCount=remainedCallDto.TotalCount;
                }
                else if (rasad.QueueCode == 25)
                {
                    remainedCallDto = await _remainedCallQueryService.GetRemainedCall(new GetRemainedCallQuery() { FromDate = fromDate, ToDate = toDate, RemainedCallType = RemainedCallType.Tamdid });

                    rasad.TotalRemainedCount = remainedCallDto.TotalRemainedCount;
                    rasad.TotalCount = remainedCallDto.TotalCount;
                }
            }

            return rasadResult;
        }

    }
}

