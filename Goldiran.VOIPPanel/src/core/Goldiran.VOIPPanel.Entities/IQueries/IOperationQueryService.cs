using Voip.Framework.Domain;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;

public interface IOperationQueryService : IBaseQueryService
{
    public Task<OperationDto> GetOperationById(long id);
    public Task<PaginatedList<OperationDto>> GetOperations(GetOperationsQuery filter);
    public Task<OperationDto> GetOperation(GetOperationQuery filter);
    public Task<PaginatedList<AggregateOperationDto>> GetGroupOperations(GetGroupOperationsQuery filter);
}
