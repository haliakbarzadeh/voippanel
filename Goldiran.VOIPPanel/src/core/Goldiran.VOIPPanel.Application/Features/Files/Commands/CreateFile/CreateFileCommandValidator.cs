using FluentValidation;
using Voip.Framework.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Voip.Framework.Common.Extensions;
using Goldiran.VOIPPanel.Application.Common.Utils;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;

namespace Goldiran.VOIPPanel.Application.Features.Files.Commands.CreateFile;


public class CreateFileCommandValidator : AbstractValidator<CreateFileCommand>
{
    public CreateFileCommandValidator( )
    {

        RuleFor(v => v.FileOwnerTypeId)
            .NotNullWithMessage("نوع مالک فایل")
            .NotEmptyWithMessage("نوع مالک فایل");

        RuleFor(v => v)
            .Must(BeSize).WithMessage("حجم فایل بایستی کمتر از 50 کیلو بایت باشد");

    }

    private bool BeSize(CreateFileCommand command)
    {
        if(command.Content.Length>51200 && command.FileOwnerTypeId==FileOwnerType.Users)
            return false;
        else
             return true;
    }

}