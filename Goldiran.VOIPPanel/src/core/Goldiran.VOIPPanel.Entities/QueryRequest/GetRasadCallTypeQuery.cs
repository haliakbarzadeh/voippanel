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

public class GetRasadCallTypeQuery : IRequest<RasadCallTypeDto>
{
    public string Agent {  get; set; }
}