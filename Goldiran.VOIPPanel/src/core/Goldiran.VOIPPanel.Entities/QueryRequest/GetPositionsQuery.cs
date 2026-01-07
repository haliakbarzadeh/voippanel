using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.ReadModel.Dto;

namespace Goldiran.VOIPPanel.ReadModel.QueryRequest;

public class GetPositionsQuery : BaseQueryRequest, IRequest<PaginatedList<PositionDto>>
{
    [BindNever]
    public bool IsRestricted { get; set; } = false;
    public string Title { get; set; }=string.Empty;
    public string PersianFullName { get; set; } = string.Empty; 
    public PositionType? PositionType { get; set; }
}