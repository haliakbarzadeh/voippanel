using Voip.Framework.Domain.Models.CQRS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.ReadModel.Dto;

namespace Goldiran.VOIPPanel.ReadModel.QueryRequest;

public class GetMonitoredPositionsQuery : BaseQueryRequest, IRequest<PaginatedList<MonitoredPositionDto>>
{
    public long? SourcePositionId { get; set; }
    public long? DestPositionId { get; set; }
}
