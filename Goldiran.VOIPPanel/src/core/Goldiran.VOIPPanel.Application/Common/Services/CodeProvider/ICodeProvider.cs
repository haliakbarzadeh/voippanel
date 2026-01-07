using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Application.Common.Services.CodeProvider
{
    public interface ICodeProvider
    {
        public string GetVerificationCode(string userName, long? positionId = null);
        public string VerifyVerificationCode(string userName, string code);
    }
}
