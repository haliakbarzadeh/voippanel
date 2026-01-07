using FluentValidation;
using Voip.Framework.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Account.ChangePasswordCommand;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{

    public ChangePasswordCommandValidator()
    {

        RuleFor(v => v.UserId)
            .NotNullWithMessage("نام کاربری");


        RuleFor(v => v.CurrentPassword)
            .NotNullWithMessage("رمز عبور فعلی")
            .NotEmptyWithMessage("رمز عبور فعلی");

        RuleFor(v => v.NewPassword)
        .NotNullWithMessage("رمز عبور جدید")
        .NotEmptyWithMessage("رمز عبور جدید");

        RuleFor(v => v.ReNewPassword)
        .NotNullWithMessage("تکرار رمز عبور جدید")
        .NotEmptyWithMessage("تکرار رمز عبور جدید");

        RuleFor(v => v)
            .Must(BeUnique).WithMessage("عدم تطابق رمز عبور جدید و تکرار رمز عبور جدید");

        RuleFor(v => v)
        .Must(BeValidatePassword)
        .WithMessage("رمز عبور بایستی حداقل 8 کاراکتر و حداکثر 15 کاراکتر باشد \r\n " +
        "رمز عبور بایستی شامل حداقل یک حرف کوچک انگلیسی باشد. \r\n "
        + "رمز عبور بایستی شامل حداقل یک حرف بزرگ انگلیسی باشد. \r\n"
        + "رمز عبور بایستی شامل حداقل یک رقم از صفر تا 9 باشد. \r\n"
        + "رمز عبور بایستی شامل حداقل یکی از دو علامت @ یا $ باشد.");
        //.WithMessage("رمز عبور بایستی شامل حداقل یک حرف کوچک انگلیسی باشد.")
        //.WithMessage("رمز عبور بایستی شامل حداقل یک حرف بزرگ انگلیسی باشد.")
        //        .WithMessage("رمز عبور بایستی شامل حداقل یک رقم از صفر تا 9 باشد.")
        //                .WithMessage("رمز عبور بایستی شامل حداقل یکی از دو علامت @ یا $ باشد.");

    }

    private bool BeUnique(ChangePasswordCommand changepass)
    {
        return changepass.NewPassword == changepass.ReNewPassword;
    }

    private bool BeValidatePassword(ChangePasswordCommand changepass)
    {
        //^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$
        Regex validateGuidRegex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,15}$");
        return validateGuidRegex.IsMatch(changepass.NewPassword);
    }

}
