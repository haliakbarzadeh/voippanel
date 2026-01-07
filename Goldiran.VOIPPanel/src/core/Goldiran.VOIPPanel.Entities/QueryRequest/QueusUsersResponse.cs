using Goldiran.VOIPPanel.ReadModel.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.QueryRequest;

public class QueusUsersResponse
{
    public IList<QueuUserDto> AddedUsers { get; set; }=new List<QueuUserDto>();
    public IList<QueuUserDto> RemovedUsers { get; set; } = new List<QueuUserDto>();
}
