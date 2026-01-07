using FluentValidation;
using Voip.Framework.Common.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Roles.Commands.UpdateRole;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmptyWithMessage("نقش")
            .NotNullWithMessage("نقش")
            .MaximumLengthWithMessage(256);

        //RuleFor(v => v)
        //    .MustAsync(BeUnique).WithMessage("نقش مربوطه قبلاً ثبت شده و امکان ثبت مجدد نیست");

    }

    //private async Task<bool> BeUnique(UpdateRoleCommand command, CancellationToken cancellationToken)
    //{
    //    return await _roleManager.Roles
    //        .Where(x => x.Id != command.Id)
    //        .AllAsync(x => x.Name != command.Name, cancellationToken);
    //}
}
