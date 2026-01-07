using FluentValidation;
using Voip.Framework.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.Application.Features.MonitoredPositions.Commands.CreateMonitoredPosition;

public class CreateMonitoredPositionCommandValidator : AbstractValidator<CreateMonitoredPositionCommand>
{

    public CreateMonitoredPositionCommandValidator()
    {
        RuleFor(v => v.SourcePositionId)
            .NotNullWithMessage("سمت مبدا");
    }

}




