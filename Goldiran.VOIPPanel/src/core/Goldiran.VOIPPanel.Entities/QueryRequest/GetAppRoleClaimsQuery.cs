using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.QueryRequest;

public class GetAppRoleClaimsQuery 
{
    public long? UserId { get; set; }
    public long? RoleId { get; set; }
}