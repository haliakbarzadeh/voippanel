using Goldiran.VOIPPanel.Application.Common.Services.IdentityService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Goldiran.VOIPPanel.Api.Controllers.UserManagement;


public class CaptchaController : ApiControllerBase
{
    ICaptchaService _captchaService;
    public CaptchaController(ICaptchaService captchaService)
    {
        _captchaService= captchaService;
    }

    [AllowAnonymous]
    [HttpGet]
    public FileResult Get()
    {
        var captcha =_captchaService.GenerateCaptcha();
       
        return File(captcha, "Image/Png");

    }
 
}