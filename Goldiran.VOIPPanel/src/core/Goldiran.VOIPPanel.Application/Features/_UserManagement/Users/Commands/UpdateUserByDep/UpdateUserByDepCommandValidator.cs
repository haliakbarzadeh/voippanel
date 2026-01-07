using FluentValidation;
using Framework.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Saramad.Core.ApplicationService.Common.ExtensionMethod;
using Saramad.Core.ApplicationService.Common.Interfaces;
using Saramad.Core.ApplicationService.Common.Utils;
using Saramad.Core.Domain.Enums;
using System.Text.RegularExpressions;

namespace Saramad.Core.ApplicationService.Features.Users.Commands.UpdateUserByDep;


public class UpdateUserByDepCommandValidator : AbstractValidator<UpdateUserByDepCommand>
{

    public UpdateUserByDepCommandValidator()
    {

        RuleFor(v => v.UserName)
            .NotNullWithMessage("نام کاربری")
            .MaximumLengthWithMessage(256, "نام کاربری");

        RuleFor(v => v.Email)
            .NotNullWithMessage("ای میل")
            .MaximumLengthWithMessage(256, "ای میل")
            .Must(BeEmailAddress).WithMessage("آدرس  ای میل وارد شده معتبر نیست");

        RuleFor(v => v.Mobile)
    .NotNullWithMessage("شماره موبایل")
    .MaximumLengthWithMessage(11, "شماره موبایل")
    .Must(PersonUtility.IsMobileNumber).WithMessage("شماره موبایل وارد شده معتبر نیست");

        RuleFor(v => v.PhoneNumber)
.Must(PersonUtility.IsPhoneNumber).WithMessage("شماره تلفن وارد شده معتبر نیست");

        RuleFor(v => v.Fax)
.Must(PersonUtility.IsPhoneNumber).WithMessage("شماره تلفن وارد شده معتبر نیست");

        RuleFor(v => v.NationalCode)
        .NotNullWithMessage("کد ملی")
             .MustAsync(BeNationalNumber).WithMessage("کد ملی صحیح را وارد کنید");

        RuleFor(v => v.OrganizationTypeId)
        .NotNullWithMessage("نوع سازمان");
    }

    private async Task<bool> BeNationalNumber(string nationalCode, CancellationToken cancellationToken)
    {
        return PersonUtility.IsNationalCodeValidation(nationalCode);
    }

    private bool BeEmailAddress(string email)
    {
        return email.IsEmailAddress();
    }
}




