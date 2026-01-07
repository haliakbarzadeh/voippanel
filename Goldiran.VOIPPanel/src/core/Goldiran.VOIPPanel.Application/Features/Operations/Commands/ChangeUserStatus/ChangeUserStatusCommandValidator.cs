using FluentValidation;
using Voip.Framework.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.Application.Features.Positions.Commands.CreatePosition;


public class ChangeUserStatusCommandValidator : AbstractValidator<ChangeUserStatusCommand>
{
    public ChangeUserStatusCommandValidator()
    {
        RuleFor(v => v.OperationTypeId)
            .NotNullWithMessage("وضعیت کاربر");

    }

}




