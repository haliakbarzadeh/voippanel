using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Roles.Commands.ChangeAccesses;


public class ChangeAccessesCommandValidator : AbstractValidator<ChangeAccessesCommand>
{

    public ChangeAccessesCommandValidator()
    {

        RuleFor(v => v.Id)
        .NotNullWithMessage("نقش");
    }

}




