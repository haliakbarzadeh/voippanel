using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Saramad.Core.ApplicationService.Common.ExtensionMethod;
using Saramad.Core.ApplicationService.Common.Interfaces;
using Saramad.Core.ApplicationService.Features.Accounts.Commands.CreateAccount;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Account.RemoveTwoFactorCommand;

public class RemoveTwoFactorCommandValidator : AbstractValidator<RemoveTwoFactorCommand>
{

    public RemoveTwoFactorCommandValidator()
    {

        RuleFor(v => v.UserId)
            .NotNullWithMessage("نام کاربری");


        RuleFor(v => v.Code)
            .NotNullWithMessage("کد")
            .NotEmptyWithMessage("کد");

    }

}
