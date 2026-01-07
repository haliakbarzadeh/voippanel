using Voip.Framework.Domain.Models.CQRS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.ReadModel.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Goldiran.VOIPPanel.ReadModel.Dto.Voip;

namespace Goldiran.VOIPPanel.ReadModel.QueryRequest;

public class GetWallboardsQuery : BaseQueryRequest, IRequest<WallboardReportDto>
{
    public WallboardReportType wallboardReportType { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    [BindNever]
    public bool? IsMorningShift { get; set; }
    [BindNever]
    public IList<long>? PositionIds { get; set; }=new List<long>();
}