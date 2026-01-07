using Voip.Framework.Domain;
using Voip.Framework.Domain.Models.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;

namespace Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;

public interface IMonitoredPositionQueryService: IBaseQueryService
{
    public Task<List<MonitoredPositionDto>> GetMonitoredPositionById(long id);
    public Task<PaginatedList<GroupMonitoredPositionDto>> GetGroupMonitoredPosition(GetGroupMonitoredPositionsQuery filter);
    public Task<PaginatedList<MonitoredPositionDto>> GetMonitoredPositions(GetMonitoredPositionsQuery filter);
}
