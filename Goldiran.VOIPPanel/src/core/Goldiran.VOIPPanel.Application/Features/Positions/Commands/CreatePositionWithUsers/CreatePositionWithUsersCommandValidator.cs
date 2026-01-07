using FluentValidation;
using Voip.Framework.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Goldiran.VOIPPanel.Application.Features.Positions.Commands.CreatePositionWithUsers;


public class CreatePositionWithUsersCommandValidator : AbstractValidator<CreatePositionWithUsersCommand>
{

    public CreatePositionWithUsersCommandValidator()
    {
        RuleFor(v => v.Title)
            .NotNullWithMessage("عنوان")
            .NotEmptyWithMessage("عنوان")
            .MaximumLengthWithMessage(100, "عنوان");

        RuleFor(v => v.PositionTypeId)
            .NotNullWithMessage("نوع سمت سازمانی");

        //RuleFor(v => v.DepartmentId)
        //    .NotNullWithMessage("نوع واحد سازمانی");
    }

}




