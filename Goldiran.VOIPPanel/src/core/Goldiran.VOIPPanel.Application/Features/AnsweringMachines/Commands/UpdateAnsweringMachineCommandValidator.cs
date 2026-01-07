using FluentValidation;
using Voip.Framework.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.Application.Features.AnsweringMachines.Commands;

public class UpdateAnsweringMachineCommandValidator : AbstractValidator<UpdateAnsweringMachineCommand>
{

    public UpdateAnsweringMachineCommandValidator()
    {
        RuleFor(v => v.Status)
            .NotNullWithMessage("وضعیت");

    }

}




