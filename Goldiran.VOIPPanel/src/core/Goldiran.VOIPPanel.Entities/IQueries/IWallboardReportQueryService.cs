using Voip.Framework.Domain;
using Voip.Framework.Domain.Models.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.Dto.Voip;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;

public interface IWallboardReportQueryService : IBaseQueryService
{
    public Task<WallboardReportDto> GetAgentContactsCount(GetWallboardsQuery filter);
    public Task<WallboardReportDto> GetCallAverageTime(GetWallboardsQuery filter);
    public Task<WallboardReportDto> GetCallSumTime(GetWallboardsQuery filter);
    public Task<WallboardReportDto> GetCSAT(GetWallboardsQuery filter);
    public Task<WallboardReportDto> GetOperationStatus(GetWallboardsQuery filter);


}
