using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Saramad.Core.ApplicationService.Common.ExtensionMethod;
using Saramad.Core.ApplicationService.Common.Interfaces;
using Saramad.Core.ApplicationService.Common.Utils;
using Saramad.Core.ApplicationService.Features.Accounts.Commands.CreateAccount;
using Saramad.Core.Domain.Enums;
using System.Text.RegularExpressions;

namespace Saramad.Core.ApplicationService.Features.Users.Commands.SetUserPassword;


public class SetUserPasswordCommandValidator : AbstractValidator<SetUserPasswordCommand>
{

    public SetUserPasswordCommandValidator()
    {

        RuleFor(v => v.UserId)
            .NotNullWithMessage("کاربر");

        //RuleFor(v => v)
        // .Must(BeUniquePassword).WithMessage("رمز عبور و تکرار رمز عبور با هم مطابقت ندارند");
    }

    //private bool BeUniquePassword(SetUserPasswordCommand command)
    //{
    //    return command.Password == command.RePassword;
    //}

}




