using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Dto.Voip;

public class WallboardReportDto
{
    public IList<WallboardDto> ItemList { get; set; }=new List<WallboardDto>();
}
public class WallboardDto
{
    public string UserName {  get; set; }=string.Empty;
    public string FullName { get; set; } = string.Empty;
    public int Value { get; set; }
    public string ValueStr { get; set; } = string.Empty;
    public string? Image { get; set; } 

}
