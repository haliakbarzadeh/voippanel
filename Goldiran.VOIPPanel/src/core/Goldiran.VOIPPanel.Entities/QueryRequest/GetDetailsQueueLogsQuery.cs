using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.Entities.Asterisk;
using Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;
using Goldiran.VOIPPanel.ReadModel.Enums;

namespace Goldiran.VOIPPanel.ReadModel.QueryRequest;

public class GetDetailsQueueLogsQuery : BaseQueryRequest, IRequest<QueueLogDetailsDto>
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public IList<int> Queues { get; set; }=new List<int>();

}
