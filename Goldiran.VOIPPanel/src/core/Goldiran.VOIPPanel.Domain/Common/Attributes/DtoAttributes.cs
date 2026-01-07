using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Domain.Common.Attributes
{
    public class DtoAttributes:Attribute
    {
        public string Name { get; set; }
        public bool IsDisplayed { get; set; }
        public DtoAttributes(string name, bool isDisplayed)
        {
            Name = name;
            IsDisplayed = isDisplayed;
        }

    }
}

