using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Saramad.Core.ApplicationService.Common.ExtensionMethod;
using Saramad.Core.ApplicationService.Common.Interfaces;
using Saramad.Core.ApplicationService.Features.Account.LoginWithOTPCommand;
using Saramad.Core.ApplicationService.Features.Idenetity.Models;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Account.VerifyLoginWithOTPCommand;

public class LoginWithOTPCommandValidator : AbstractValidator<VerifyLoginWithOTPCommand>
{

    public LoginWithOTPCommandValidator()
    {

        RuleFor(v => v.UserId)
            .NotNullWithMessage("نام کاربری");


        RuleFor(v => v.Code)
            .NotNullWithMessage("کد عبور ")
            .NotEmptyWithMessage("کد عبور");



    }
}
