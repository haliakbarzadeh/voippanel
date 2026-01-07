using Goldiran.VOIPPanel.ReadModel.Dto;
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

public class GetRemainedCallQuery : BaseQueryRequest, IRequest<RemainedCallDto>
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public bool? IsTehran { get; set; }
    public bool IsTotall { get; set; }=false;

    public RemainedCallType RemainedCallType { get; set; }=RemainedCallType.Marketting;

}
