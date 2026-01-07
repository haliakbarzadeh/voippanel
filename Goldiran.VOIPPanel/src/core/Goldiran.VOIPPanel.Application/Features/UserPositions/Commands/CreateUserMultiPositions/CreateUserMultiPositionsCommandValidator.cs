using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Saramad.Core.ApplicationService.Common.ExtensionMethod;
using Saramad.Core.ApplicationService.Common.Interfaces;
using Saramad.Core.ApplicationService.Common.Utils;
using Saramad.Core.Domain.Enums;
using System.Text.RegularExpressions;

namespace Saramad.Core.ApplicationService.Features.UserMultiPositionss.Commands.CreateUserMultiPositions;


public class CreateUserMultiPositionsCommandValidator : AbstractValidator<CreateUserMultiPositionsCommand>
{

    public CreateUserMultiPositionsCommandValidator()
    {
        RuleFor(v => v.UserId)
            .NotNullWithMessage("کاربر");

        RuleFor(v => v.PositionId)
            .NotNullWithMessage("سمت سازمانی");
    }

}




