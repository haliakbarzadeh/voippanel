using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Voip.Framework.Caching.Caching;
using Voip.Framework.Common.AppSettings;

namespace Goldiran.VOIPPanel.Application.Common.Services.CodeProvider
{
    public class CodeProvider : ICodeProvider
    {
        private readonly IConfiguration _configuration;
        //private readonly IMemoryCache _memoryCache;
        private readonly IOptionsSnapshot<SiteSettings> _siteOptions;
        private readonly IDistributedCache _cach;

        public CodeProvider(IConfiguration configuration,  IOptionsSnapshot<SiteSettings> siteOptions, IDistributedCache cach)
        {
            _configuration = configuration;
            //_memoryCache = memoryCache;
            _siteOptions = siteOptions;
            _cach = cach;
            //memoryCache.Set<string>("1", "", TimeSpan.FromMinutes(_configuration.GetSection("VerificationCodeExpireTime").GetValue<int>("Ds")));
        }

        public string GetVerificationCode(string userName, long? positionId=null)
        {
            string cachResult = null;

            //try
            //{
            //    cachResult = _cach.Get<string>(userName);
            //    if(cachResult!=null)
            //    {
            //        return cachResult;
            //    }

            //}
            //catch (Exception)
            //{

            //}
            //بایستی تغییر کند
            if (_siteOptions.Value.Enviroment == "pilot")
            {
                if (positionId != null)
                {
                    cachResult = "123456" + ';' + positionId;
                }
                else
                {
                    cachResult = "123456";
                }
            }
            else
            {
                if (positionId != null)
                {
                    cachResult = new Random().Next(100000, 1000000).ToString() + ';' + positionId;
                }
                else
                {
                    cachResult = new Random().Next(100000, 1000000).ToString();
                }

            }


            //بایستی حذف شود
            //if (userName.ToLower() == "p_salimi" || userName.ToLower() == "a_biabani")
            //{
            //    if (positionId != null)
            //    {
            //        cachResult = "123456" + ';' + positionId;
            //    }
            //    else
            //    {
            //        cachResult = "123456";
            //    }
            //}

            _cach.Set(cachResult, userName, _siteOptions.Value.Cach.miuteOffset);
            //_cach.Set(cachResult, userName, 3);

            return cachResult;
        }

        public string VerifyVerificationCode(string userName, string code)
        {
            string cachResult = null;

            try
            {
                cachResult = _cach.Get<string>(userName);
                if(cachResult==null) { return string.Empty; }
            }
            catch (Exception)
            {
                return string.Empty;
            }

            if (cachResult.Split(';')[0] == code)
            {
                return cachResult;
            }
            else
            {
                return string.Empty;
            }
        }

        private TimeSpan GetChannelExpireTime()
        {
            return TimeSpan.FromMinutes(_siteOptions.Value.VerificationCodeExpireTime);
        }
    }


}

