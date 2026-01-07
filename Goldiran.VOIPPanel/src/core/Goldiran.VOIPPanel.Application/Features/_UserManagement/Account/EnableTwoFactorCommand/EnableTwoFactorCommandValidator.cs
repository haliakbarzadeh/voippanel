using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Saramad.Core.ApplicationService.Common.ExtensionMethod;
using Saramad.Core.ApplicationService.Common.Interfaces;
using Saramad.Core.ApplicationService.Features.Accounts.Commands.CreateAccount;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Account.EnableTwoFactorCommand;

public class EnableTwoFactorCommandValidator : AbstractValidator<EnableTwoFactorCommand>
{

    public EnableTwoFactorCommandValidator()
    {

        RuleFor(v => v.UserId)
            .NotNullWithMessage("نام کاربری");


        RuleFor(v => v.Code)
            .NotNullWithMessage("کد")
            .NotEmptyWithMessage("کد");

    }

}
