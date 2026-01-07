using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saramad.Core.ApplicationService.Features._Idenetity._Models
{
    public class GetTwoFactorResponse
    {
        public string QrUri { get; set; }
        public string ManualKey { get; set; }
    }
}
