using Voip.Framework.Domain;
using Voip.Framework.Domain.Models.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions.Contracts;

public interface IUserPositionQueryService : IBaseQueryService
{
    public Task<UserPositionDto> GetUserPositionById(long id);
    public Task<PaginatedList<UserPositionDto>> GetUserPositions(GetUserPositionsQuery filter);
    public Task<PaginatedList<UserPositionDto>> GetUserPositions(GetOperationUsersQuery filter);
    //public Task<List<UserPosition>> GetUserPositionsList(GetUserPositionsListQuery filter);
}
