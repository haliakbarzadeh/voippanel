using FluentValidation;
using Voip.Framework.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Account.RefreshCommand;

public class RefreshCommandValidator : AbstractValidator<RefreshCommand>
{

    public RefreshCommandValidator()
    {

        //RuleFor(v => v.AccessToken)
        //    .NotNullWithMessage("توکن")
        //    .NotEmptyWithMessage("توکن");


        RuleFor(v => v.RefreshToken)
            .NotNullWithMessage("به روزرسانی توکن")
            .NotEmptyWithMessage("به روزرسانی توکن");



    }
}
