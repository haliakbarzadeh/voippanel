using FluentValidation;
using Voip.Framework.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.Application.Features.Queus.Commands.CreateQueu;

public class CreateQueuCommandValidator : AbstractValidator<CreateQueuCommand>
{

    public CreateQueuCommandValidator()
    {
        RuleFor(v => v.Code)
            .NotNullWithMessage("کد صف");
        RuleFor(v => v.Name)
            .NotNullWithMessage("نام صف");
    }

}




