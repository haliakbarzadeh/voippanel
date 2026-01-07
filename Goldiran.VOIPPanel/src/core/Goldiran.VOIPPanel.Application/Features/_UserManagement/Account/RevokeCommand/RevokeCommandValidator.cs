using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Saramad.Core.ApplicationService.Common.ExtensionMethod;
using Saramad.Core.ApplicationService.Common.Interfaces;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Account.RevokeCommand;

public class RevokeCommandValidator : AbstractValidator<RevokeCommand>
{

    public RevokeCommandValidator()
    {

        RuleFor(v => v.UserId)
            .NotNullWithMessage("کاربر");

    }
}
