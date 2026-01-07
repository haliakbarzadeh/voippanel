using FluentValidation;
using Voip.Framework.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Goldiran.VOIPPanel.Application.Features.Positions.Commands.ChangeGroupUserStatus;


public class ChangeGroupUserStatusCommandValidator : AbstractValidator<ChangeGroupUserStatusCommand>
{
    public ChangeGroupUserStatusCommandValidator()
    {
        RuleFor(v => v.OperationTypeId)
            .NotNullWithMessage("وضعیت کاربر");
        RuleFor(v => v.ChangeReason)
            .NotNullWithMessage("دلیل تغییر وضعیت");
    }

}




