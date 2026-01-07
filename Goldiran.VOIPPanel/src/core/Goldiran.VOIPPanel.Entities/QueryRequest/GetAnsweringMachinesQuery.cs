using Goldiran.VOIPPanel.Domain.AggregatesModel.AnsweringMachines.Enums;
using Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;
using Goldiran.VOIPPanel.ReadModel.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Domain.Models.CQRS;

namespace Goldiran.VOIPPanel.ReadModel.QueryRequest;

public class GetAnsweringMachinesQuery : BaseQueryRequest, IRequest<PaginatedList<AnsweringMachineDto>>
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public TimeSpan? FromTime { get; set; } = new TimeSpan(0, 0, 0);
    public TimeSpan? ToTime { get; set; } = new TimeSpan(23, 59, 59);
    public string? Number { get; set; }
    public AMStatusType? AMStatus { get; set; }


}
