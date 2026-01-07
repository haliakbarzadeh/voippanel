using FluentValidation;
using Voip.Framework.Common.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Roles.Commands.CreateRole;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmptyWithMessage("نقش")
            .MaximumLengthWithMessage(256);
    }



}
