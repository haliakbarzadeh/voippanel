using FluentValidation;
using Voip.Framework.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Goldiran.VOIPPanel.Application.Features.OperationSettings.Commands.CreateOperationSetting;

public class CreateOperationSettingCommandValidator : AbstractValidator<CreateOperationSettingCommand>
{

    public CreateOperationSettingCommandValidator()
    {
        RuleFor(v => v.OperationTypeId)
            .NotNullWithMessage("نوع عملیات");
        RuleFor(v => v.Order)
            .NotNullWithMessage("درجه مرتب سازی");
        RuleFor(v => v.Name)
            .NotNullWithMessage("نام عملیات");
    }

}




