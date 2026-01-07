using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Saramad.Core.ApplicationService.Common.ExtensionMethod;
using Saramad.Core.ApplicationService.Common.Interfaces;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Account.TwoFactorLoginCommand;

public class TwoFactorLoginCommandValidator : AbstractValidator<TwoFactorLoginCommand>
{

    public TwoFactorLoginCommandValidator()
    {

        RuleFor(v => v.Code)
            .NotNullWithMessage("کد")
            .NotEmptyWithMessage("کد");

    }
}
