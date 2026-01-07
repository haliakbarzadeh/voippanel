using Voip.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Entities.Identity
{
    public class Token : BaseQueryEntity<long>
    {
        public long? UserId { get; set; }
        public virtual AppUser User { get; set; }
        public string? RefreshToken { get; set; }
        public string? TokenValue { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
