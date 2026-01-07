using Voip.Framework.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Goldiran.VOIPPanel.ReadModel.Entities
{
    public class AppUserUsedPassword : BaseQueryEntity<long>
    {
        public string HashedPassword { get; set; }
        public virtual AppUser User { get; set; }
        public long UserId { get; set; }

    }
}