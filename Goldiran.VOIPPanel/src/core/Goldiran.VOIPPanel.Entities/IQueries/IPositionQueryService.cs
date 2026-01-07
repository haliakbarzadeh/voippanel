using Voip.Framework.Domain;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Contracts;

public interface IPositionQueryService : IBaseQueryService
{
    public Task<PositionDto> GetPositionById(long id);
    public Task<PaginatedList<PositionDto>> GetPositions(GetPositionsQuery filter);
}
