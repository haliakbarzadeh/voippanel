using Voip.Framework.Domain;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.Queus.Contracts;

public interface IQueuQueryService : IBaseQueryService
{
    public Task<PaginatedList<QueuDto>> GetQueus(GetQueusQuery filter);
    public Task<PaginatedList<QueuDto>> GetPositionQueues(GetPositionQueusQuery filter);
    public Task<PaginatedList<QueuUserDto>> GetQueusUsers(GetQueusUsersQuery filter);
    public Task<QueusUsersResponse> GetSeperatedQueusUsers(GetSeperatedQueusUsersQuery filter);
}
