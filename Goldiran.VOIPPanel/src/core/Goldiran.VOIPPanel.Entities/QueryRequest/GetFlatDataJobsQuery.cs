using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.Domain.AggregatesModel.ContactDetails.Enums;

namespace Goldiran.VOIPPanel.ReadModel.QueryRequest;

public class GetFlatDataJobLastQuery : IRequest<FlatDataJobDto>
{
    public ReportType? ReportType { get; set; }
}