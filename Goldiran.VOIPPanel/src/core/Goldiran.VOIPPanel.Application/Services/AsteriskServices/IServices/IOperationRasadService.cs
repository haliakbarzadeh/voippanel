using Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Application.Services.AsteriskServices.IServices;

public interface IOperationRasadService
{
    public Task<List<OperationRasadResponse>> GetOperationRasad(OperationRasadRequest request);
}
