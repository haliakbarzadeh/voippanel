using Voip.Framework.Domain;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations.Contracts;

public interface IQueueLimitationQueryService : IBaseQueryService
{
    public Task<QueueLimitationDto> GetQueueLimitationById(int id);
    public Task<PaginatedList<QueueLimitationDto>> GetQueueLimitations(GetQueueLimitationsQuery filter);
}
