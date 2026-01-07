using Voip.Framework.Domain;
using Voip.Framework.Domain.Models.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;

namespace Goldiran.VOIPPanel.ReadModel.Queries.IQueryServices;

public interface IQueueLogQueryService : IBaseQueryService
{
    public Task<PaginatedList<QueueLogDto>> GetQueueLogs(GetQueueLogsQuery filter);
}
