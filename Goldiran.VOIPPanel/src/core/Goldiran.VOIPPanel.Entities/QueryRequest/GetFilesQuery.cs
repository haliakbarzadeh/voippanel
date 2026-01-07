using Voip.Framework.Domain.Models.CQRS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;

namespace Goldiran.VOIPPanel.ReadModel.QueryRequest;

public class GetFilesQuery : BaseQueryRequest, IRequest<PaginatedList<FileRowDto>>
{
    public Guid? FileOwnerId { get; set; }
    public FileOwnerType? FileOwnerTypeId { get; set; }
}