using System.Collections.Generic;

namespace Goldiran.VOIPPanel.Application.Common.Services.IdentityService
{
    public class IdentityCommonResponse
    {
        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; }
    }
}
