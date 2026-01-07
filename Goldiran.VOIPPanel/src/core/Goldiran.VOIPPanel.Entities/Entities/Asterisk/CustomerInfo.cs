using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Entities.Asterisk;

public class CustomerInfo
{
    public long Id { get; set; }

    public string CityNameId {  get; set; }=string.Empty;
    public string CustomerCD { get; set;}=string.Empty;
    public string Number { get; set; } =string.Empty;
    public int Region { get; set; }

}
