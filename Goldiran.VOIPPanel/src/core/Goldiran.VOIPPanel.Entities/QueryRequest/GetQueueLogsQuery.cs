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

public class GetQueueLogsQuery : BaseQueryRequest, IRequest<PaginatedList<QueueLogDto>>
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public TimeSpan? FromTime { get; set; }
    public TimeSpan? ToTime { get; set; }
    public IList<int>? Agents { get; set; } = new List<int>();
    public string? Phone { get; set; }
    public IList<int> Queues { get; set; }=new List<int>();
    public IList<QueueLogEventType> QueueLogEvents { get; set; }=new List<QueueLogEventType>();

}
