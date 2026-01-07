using FluentValidation;
using Voip.Framework.Common.Exceptions;
using Goldiran.VOIPPanel.Application.Features.QueueLimitations.Commands.UpdateQueueLimitation;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Goldiran.VOIPPanel.Application.Features.QueueLimitations.Commands.UpdateQueueLimitationSetting;
public class UpdateQueueLimitationCommandValidator : AbstractValidator<UpdateQueueLimitationCommand>
{

    public UpdateQueueLimitationCommandValidator()
    {
    }

}




