using Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Application.Services.AsteriskServices.IServices;

public interface IQueueRasadService
{
    public Task<QueueRasadResponse> GetQueueRasad(QueueRasadRequest request);
}
