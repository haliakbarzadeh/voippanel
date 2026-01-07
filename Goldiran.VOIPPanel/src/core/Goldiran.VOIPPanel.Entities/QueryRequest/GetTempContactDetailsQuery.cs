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

namespace Goldiran.VOIPPanel.ReadModel.QueryRequest;

public class GetTempContactDetailsQuery : BaseQueryRequest, IRequest<PaginatedList<ContactDetailDto>>
{
    [BindNever]
    public bool IsRestricted { get; set; } = true;
    [BindNever]
    public ContactReportType ContactReportType { get; set; } = ContactReportType.Detail;
    [BindNever]
    public bool IsJob { get; set; } = false;
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public TimeSpan? FromTime { get; set; }
    public TimeSpan? ToTime { get; set; }
    //public List<int>? TypeCalls { get; set; } = new List<int>();
    //public List<int>? Agents { get; set; } = new List<int>();
    public IList<int> TypeCalls { get; set; }=new List<int>();
    public string Agents { get; set; }=string.Empty;
    public string? Phone { get; set; }
    public int? OrderBy { get; set; }
    public OperationReportType? OperationReportType { get; set; }

}
