using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saramad.Core.ApplicationService.Features.Idenetity.Models
{
    public class LoginResponse
    {
        public bool IsTemporary { get; set; }
        public long? UserId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public bool IsMultiPosition { get; set; }=false;
        public List<LoginPositions> PositionList { get; set; }=new List<LoginPositions>();
    }

    public class LoginPositions
    {
        public long? Id { get; set; }
        public string Title { get; set; }

    }
}
