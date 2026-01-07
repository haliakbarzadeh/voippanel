using Voip.Framework.Domain;
using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Entities
{
    public class LoginHistory : BaseQueryEntity<long>
    {
        public string? Ip { get; set; }
        public bool Success { get; set; }
        public LoginFunctionType LoginFunctionTypeId { get; set; }
        public DateTime ActionDate { get; set; }
        public long UserId { get; set; }
        public AppUser User { get; set; }
    }
}
