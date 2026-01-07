using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saramad.Core.ApplicationService.Features.Idenetity.Models
{
    public class InitLoginResponse
    {
        public bool IsTemporary { get; set; }
        public long UserId { get; set; }

    }
}
