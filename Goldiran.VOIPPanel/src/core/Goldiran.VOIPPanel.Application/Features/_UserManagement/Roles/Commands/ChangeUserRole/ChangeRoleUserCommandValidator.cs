using FluentValidation;
using Voip.Framework.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Roles.Commands.ChangeRoleUser;


public class ChangeRoleUserCommandValidator : AbstractValidator<ChangeRoleUserCommand>
{

    public ChangeRoleUserCommandValidator()
    {

        RuleFor(v => v.Id)
        .NotNullWithMessage("نقش");
    }

}




