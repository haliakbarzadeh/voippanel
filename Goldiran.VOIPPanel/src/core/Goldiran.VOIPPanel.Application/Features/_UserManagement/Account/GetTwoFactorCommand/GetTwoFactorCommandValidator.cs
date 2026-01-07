using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Saramad.Core.ApplicationService.Common.ExtensionMethod;
using Saramad.Core.ApplicationService.Common.Interfaces;
using Saramad.Core.ApplicationService.Features.Accounts.Commands.CreateAccount;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Account.GetTwoFactorCommand;

public class GetTwoFactorCommandValidator : AbstractValidator<GetTwoFactorCommand>
{

    public GetTwoFactorCommandValidator()
    {

        RuleFor(v => v.UserId)
            .NotNullWithMessage("نام کاربری");

    }

}
