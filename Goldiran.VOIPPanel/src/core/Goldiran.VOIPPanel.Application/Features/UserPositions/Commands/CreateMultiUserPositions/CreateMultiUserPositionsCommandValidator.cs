using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Goldiran.VOIPPanel.Application.Features.UserPositions.Commands.CreateMultiUserPositions;


public class CreateMultiUserPositionsCommandValidator : AbstractValidator<CreateMultiUserPositionsCommand>
{

    public CreateMultiUserPositionsCommandValidator()
    {
        RuleFor(v => v.UserId)
            .NotNullWithMessage("کاربر");

        RuleFor(v => v.PositionId)
            .NotNullWithMessage("سمت سازمانی");
    }

}




