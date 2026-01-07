using FluentValidation;
using Voip.Framework.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Goldiran.VOIPPanel.Application.Features.Rasads.Commands.MarkettingRasad;


public class MarkettingRasadCommandValidator : AbstractValidator<MarkettingRasadCommand>
{
    public MarkettingRasadCommandValidator()
    {

    }

}




