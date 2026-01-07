using FluentValidation;
using Voip.Framework.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Saramad.Core.ApplicationService.Features.Idenetity.Models;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Account.LoginCommand;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{

    public LoginCommandValidator()
    {

        RuleFor(v => v.UserName)
            .NotNullWithMessage("نام کاربری")
            .NotEmptyWithMessage("نام کاربری");


        RuleFor(v => v.Password)
            .NotNullWithMessage("رمز عبور")
            .NotEmptyWithMessage("رمز عبور");



    }
}
