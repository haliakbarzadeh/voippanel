using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Dto;

public class RasadOperationDto
{
    public IList<RasadTeamOperationDto>  RasadTeamOperationDtoList { get; set; }=new List<RasadTeamOperationDto>();
    public IList<RasadUserOperationDto> RasadUserOperationList { get; set; }= new List<RasadUserOperationDto>();
}
