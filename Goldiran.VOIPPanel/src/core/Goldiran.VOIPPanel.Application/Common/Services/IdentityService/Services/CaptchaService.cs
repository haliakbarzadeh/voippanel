using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SixLaborsCaptcha.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Common.AppSettings;

namespace Goldiran.VOIPPanel.Application.Common.Services.IdentityService
{
    public class CaptchaService:ICaptchaService
    {
        private readonly IDistributedCache _cach;
        private readonly ISixLaborsCaptchaModule _sixLaborsCaptcha;
        private readonly IOptionsSnapshot<SiteSettings> _siteOptions;
        public CaptchaService(IDistributedCache cach, ISixLaborsCaptchaModule sixLaborsCaptcha, IOptionsSnapshot<SiteSettings> siteOptions)
        {
            _cach= cach;
            _sixLaborsCaptcha= sixLaborsCaptcha;
            _siteOptions= siteOptions;
        }

        public byte[] GenerateCaptcha()
        {
            string key = Extensions.GetUniqueKey(_siteOptions.Value.CaptchaLenght);
            var imageStream = _sixLaborsCaptcha.Generate(key);
            _cach.Set(key.ToLower(), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(key.ToLower())), new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = new TimeSpan(0, 0, _siteOptions.Value.CaptchaTime) });
            return imageStream;
        }
    }
}
