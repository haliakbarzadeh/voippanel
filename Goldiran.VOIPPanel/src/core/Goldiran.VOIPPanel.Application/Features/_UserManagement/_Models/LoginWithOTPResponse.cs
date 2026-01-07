using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saramad.Core.ApplicationService.Features.Idenetity.Models
{
    public class LoginWithOTPResponse
    {
        public bool IsTemporary { get; set; }
        public long? UserId { get; set; }
        //public string test { get; set; }
        public bool IsMultiPosition { get; set; } = false;
        public List<LoginPositions> PositionList { get; set; } = new List<LoginPositions>();
    }
}
