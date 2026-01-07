using FluentValidation;
using Voip.Framework.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Goldiran.VOIPPanel.Application.Features.Positions.Commands.CreatePosition;


public class ChangeUserStatusBySupervisorCommandValidator : AbstractValidator<ChangeUserStatusBySupervisorCommand>
{
    public ChangeUserStatusBySupervisorCommandValidator()
    {
        RuleFor(v => v.OperationTypeId)
            .NotNullWithMessage("وضعیت کاربر");
        RuleFor(v => v.User)
            .NotNullWithMessage("کاربر");
    }

}




