using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Application.Common.Services.IdentityService
{
    public interface ICaptchaService
    {
        public byte[] GenerateCaptcha();
    }
}
