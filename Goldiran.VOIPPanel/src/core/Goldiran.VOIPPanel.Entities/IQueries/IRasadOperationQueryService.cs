using Voip.Framework.Domain;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.Operations.Contracts;

public interface IRasadOperationQueryService: IBaseQueryService
{
    public Task<RasadOperationDto> GetRasadOperation(GetRasadOperationQuery filter);
}
