using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Dto.Voip;

public class ServiceWallboardReportDto
{
    public AcceptenceDto AcceptenceRate { get; set; }
    public TargetDto ServiceTarget { get; set; }
    public TargetDto RegarantTarget { get; set; }
    public WallboardUserDto ServiceBestUser { get; set; }
    public WallboardUserDto RegarantBestUser { get; set; }
    public IList<WallboardUserDto> ServiceUsersList { get; set; }=new List<WallboardUserDto>();
    public IList<WallboardUserDto> RegarantUsersList { get; set; } = new List<WallboardUserDto>();
    public IList<ConversionRateDto> ConversionRateList { get; set; } = new List<ConversionRateDto>();


}
public class WallboardUserDto
{
    public string UserName {  get; set; }=string.Empty;
    public string PersonalID { get; set; } = string.Empty;
    public int Value { get; set; }
    public string? Image { get; set; } 

}

public class ConversionRateDto
{
    public int TimeDuration { get; set; }
    public int ServiceCount { get; set; }
    public int CallCount { get; set; }
    public int ServiceCallCount { get; set; }
    public int TamdidCount { get; set; }
    public int TamdidCallCount { get; set; }

}

public class AcceptenceDto
{
    public int ServiceAcceptance { get; set; }
    public int RegarantAcceptance { get; set; }

}

public class TargetDto
{
    public int count { get; set; }
    public int Target { get; set; }

}
