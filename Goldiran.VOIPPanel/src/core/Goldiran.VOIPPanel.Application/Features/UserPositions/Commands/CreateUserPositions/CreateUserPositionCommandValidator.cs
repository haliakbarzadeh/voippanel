using FluentValidation;
using Voip.Framework.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.Application.Features.UserPositions.Commands.CreateUserPosition;

public class CreateUserPositionCommandValidator : AbstractValidator<CreateUserPositionCommand>
{

    public CreateUserPositionCommandValidator()
    {
        RuleFor(v => v.UserId)
            .NotNullWithMessage("کاربر");

        RuleFor(v => v.PositionId)
            .NotNullWithMessage("سمت سازمانی");
    }

}




