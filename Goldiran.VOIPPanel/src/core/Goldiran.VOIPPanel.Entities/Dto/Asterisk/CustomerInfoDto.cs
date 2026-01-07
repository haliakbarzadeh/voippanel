using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;

public class CustomerInfoDto
{
    public long Id { get; set; }

    public string CityNameId {  get; set; }=string.Empty;
    public string CityName {  get; set; }=string.Empty;
    public string CustomerCD { get; set;}=string.Empty;
    public string Number { get; set; } =string.Empty;
    public int Region { get; set; }

}
