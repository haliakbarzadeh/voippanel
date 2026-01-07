using FluentValidation;
using Voip.Framework.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Users.Commands.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{

    public ResetPasswordCommandValidator()
    {

        RuleFor(v => v.UserName)
            .NotNullWithMessage("کاربر")
            .NotNullWithMessage("کاربر");


        RuleFor(v => v)
        .Must(BeValidatePassword)
        .WithMessage("رمز عبور بایستی حداقل 8 کاراکتر و حداکثر 15 کاراکتر باشد \r\n " +
        "رمز عبور بایستی شامل حداقل یک حرف کوچک انگلیسی باشد. \r\n "
        + "رمز عبور بایستی شامل حداقل یک حرف بزرگ انگلیسی باشد. \r\n"
        + "رمز عبور بایستی شامل حداقل یک رقم از صفر تا 9 باشد. \r\n"
        + "رمز عبور بایستی شامل حداقل یکی از دو علامت @ یا $ باشد.");

    }

    private bool BeValidatePassword(ResetPasswordCommand changepass)
    {
        //^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$
        Regex validateGuidRegex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,15}$");
        return validateGuidRegex.IsMatch(changepass.NewPassword);
    }
}




