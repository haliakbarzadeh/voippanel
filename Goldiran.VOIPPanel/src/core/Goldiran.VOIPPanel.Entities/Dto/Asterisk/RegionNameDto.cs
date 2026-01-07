using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Dto.Asterisk
{
    internal class RegionNameDto
    {
        public int Id { get; set; }
        public int CityNameId {  get; set; }
        public string CityName { get; set; }=string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
