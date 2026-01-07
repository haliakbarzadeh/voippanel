using FluentValidation;
using Voip.Framework.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Goldiran.VOIPPanel.Application.Features.Queus.Commands.UpdateQueu;
public class UpdateQueuCommandValidator : AbstractValidator<UpdateQueuCommand>
{

    public UpdateQueuCommandValidator()
    {
        RuleFor(v => v.Code)
            .NotNullWithMessage("کد صف");
        RuleFor(v => v.Name)
            .NotNullWithMessage("نام صف");
    }

}




