using FluentValidation;
using Voip.Framework.Common.Extensions;
using Goldiran.VOIPPanel.Application.Common.Utils;
using Goldiran.VOIPPanel.Application.Features.UserManagement.Account.ChangePasswordCommand;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Users.Commands.CreateUser;
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{

    public CreateUserCommandValidator()
    {

        RuleFor(v => v.UserName)
            .NotNullWithMessage("نام کاربری")
            .MaximumLengthWithMessage(256, "نام کاربری");

        RuleFor(v => v.PersianFullName)
    .NotNullWithMessage("نام و نام خانوادگی")
    .MaximumLengthWithMessage(256, "نام و نام خانوادگی");

        RuleFor(v => v.Email)
            .NotNullWithMessage("ای میل")
             .MaximumLengthWithMessage(256, "ای میل")
            .Must(BeEmailAddress).WithMessage("آدرس  ای میل وارد شده معتبر نیست");

        RuleFor(v => v.PhoneNumber)
                .NotNullWithMessage("شماره موبایل")
                    .MaximumLengthWithMessage(11, "شماره موبایل")
                .Must(PersonUtility.IsMobileNumber).WithMessage("شماره تلفن وارد شده معتبر نیست");

        //    RuleFor(v => v.PhoneNumber)
        //.Must(PersonUtility.IsPhoneNumber).WithMessage("شماره تلفن وارد شده معتبر نیست");


        RuleFor(v => v.NationalCode)
            .NotNullWithMessage("کد ملی");
        //     .MustAsync(BeNationalNumber).WithMessage("کد ملی صحیح را وارد کنید");

        RuleFor(v => v.Password)
            .NotNullWithMessage("رمز عبور")
                    .Must(BeValidatePassword)
        .WithMessage("رمز عبور بایستی حداقل 8 کاراکتر و حداکثر 15 کاراکتر باشد \r\n " +
        "رمز عبور بایستی شامل حداقل یک حرف کوچک انگلیسی باشد. \r\n "
        + "رمز عبور بایستی شامل حداقل یک حرف بزرگ انگلیسی باشد. \r\n"
        + "رمز عبور بایستی شامل حداقل یک رقم از صفر تا 9 باشد. \r\n"
        + "رمز عبور بایستی شامل حداقل یکی از دو علامت @ یا $ باشد.");

        RuleFor(v => v.ConfirmPassword)
            .NotNullWithMessage("تکرار رمز عبور");

        RuleFor(v => v)
    .Must(BeConfirmPassword).WithMessage("عدم تطابق رمز عبور و تکرار رمز عبور");


    }

    private async Task<bool> BeNationalNumber(string nationalCode, CancellationToken cancellationToken)
    {
        return PersonUtility.IsNationalCodeValidation(nationalCode);
    }

    private bool BeEmailAddress(string email)
    {
        return email.IsEmailAddress();
    }
    private bool BeValidatePassword(string password)
    {
        //^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$
        Regex validateGuidRegex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,15}$");
        return validateGuidRegex.IsMatch(password);
    }

    private bool BeConfirmPassword(CreateUserCommand password)
    {
        return password.ConfirmPassword==password.Password;
    }
}




