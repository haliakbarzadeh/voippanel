using System.ComponentModel.DataAnnotations;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories.Enums;
public enum LoginFunctionType
{
    [Display(Description = "ورود موفق")]
    SuccessLogin =1,
    [Display(Description = "خروج موفق")]
    SuccessLogout = 2,
    [Display(Description = "ورود ناموفق")]
    FailedLogin = 3,
    [Display(Description = "خروج ناموفق")]
    FailedLogout = 4,
}