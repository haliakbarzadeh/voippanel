using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Entities.Asterisk;

public class RegionName
{
    public int Id { get; set; }
    public int CityNameId {  get; set; }
    public string Name { get; set; } = string.Empty;
}
