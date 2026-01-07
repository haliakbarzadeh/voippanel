using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Saramad.Core.ApplicationService.Common.ExtensionMethod;
using Saramad.Core.ApplicationService.Common.Interfaces;
using Saramad.Core.ApplicationService.Common.Utils;
using Saramad.Core.ApplicationService.Features.Accounts.Commands.CreateAccount;
using Saramad.Core.Domain.Enums;
using System.Text.RegularExpressions;

namespace Saramad.Core.ApplicationService.Features.Users.Commands.ForgotPassword;


public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{

    public ForgotPasswordCommandValidator()
    {

        RuleFor(v => v.Username)
            .NotNullWithMessage("کاربر")
            .NotNullWithMessage("کاربر");

    }

}




