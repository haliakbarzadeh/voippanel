using FluentValidation;
using Voip.Framework.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Goldiran.VOIPPanel.Application.Features.Rasads.Commands.QueueRasad;


public class QueueRasadCommandValidator : AbstractValidator<QueueRasadCommand>
{
    public QueueRasadCommandValidator()
    {

    }

}




