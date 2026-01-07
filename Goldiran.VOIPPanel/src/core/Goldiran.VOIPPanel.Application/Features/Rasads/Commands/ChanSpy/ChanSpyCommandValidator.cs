using FluentValidation;
using Voip.Framework.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Goldiran.VOIPPanel.Application.Features.Rasads.Commands.ChanSpy;


public class ChanSpyCommandValidator : AbstractValidator<ChanSpyCommand>
{
    public ChanSpyCommandValidator()
    {
        RuleFor(v => v.Caller)
            .NotNullWithMessage("شماره خروجی");

        RuleFor(v => v.SpyType)
           .NotNullWithMessage("وضعیت");
    }

}




