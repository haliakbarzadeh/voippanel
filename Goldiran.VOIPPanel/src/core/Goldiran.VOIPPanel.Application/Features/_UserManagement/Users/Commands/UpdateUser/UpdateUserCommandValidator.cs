using FluentValidation;
using Voip.Framework.Common.Exceptions;
using Goldiran.VOIPPanel.Application.Common.Utils;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Users.Commands.UpdateUser;
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{

    public UpdateUserCommandValidator()
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

    //    RuleFor(v => v.Mobile)
    //.NotNullWithMessage("شماره موبایل")
    //.MaximumLengthWithMessage(11, "شماره موبایل")
    //.Must(PersonUtility.IsMobileNumber).WithMessage("شماره موبایل وارد شده معتبر نیست");

        RuleFor(v => v.PhoneNumber)
                .NotNullWithMessage("شماره موبایل")
                    .MaximumLengthWithMessage(11, "شماره موبایل")
.Must(PersonUtility.IsMobileNumber).WithMessage("شماره تلفن وارد شده معتبر نیست");

        //        RuleFor(v => v.Fax)
        //.Must(PersonUtility.IsPhoneNumber).WithMessage("شماره تلفن وارد شده معتبر نیست");

        RuleFor(v => v.NationalCode)
        .NotNullWithMessage("کد ملی");
             //.MustAsync(BeNationalNumber).WithMessage("کد ملی صحیح را وارد کنید");

        //RuleFor(v => v.OrganizationTypeId)
        //.NotNullWithMessage("نوع سازمان");
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




