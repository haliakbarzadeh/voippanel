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

public class GetQueusUsersQuery : BaseQueryRequest, IRequest<PaginatedList<QueuUserDto>>
{
    [BindNever]
    public int QueueId {  get; set; }
    [BindNever]
    public string QueueName { get; set; } = string.Empty;
    [BindNever]
    public List<string> QueueList { get; set; } = new List<string>();
    [BindNever]
    public IList<string> Extensions { get; set; }=new List<string>();   
}