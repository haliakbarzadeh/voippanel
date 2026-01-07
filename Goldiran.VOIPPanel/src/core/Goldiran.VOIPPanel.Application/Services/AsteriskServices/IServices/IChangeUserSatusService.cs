using Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;
using Goldiran.VOIPPanel.ReadModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Application.Services.AsteriskServices.IServices;

public interface IChangeUserSatusService
{
    public Task ChangeUserStatus(ChangeUserSattusRequest request);
}
