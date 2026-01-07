using FluentValidation;
using Voip.Framework.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Goldiran.VOIPPanel.Application.Features.QueueLimitations.Commands.CreateQueueLimitation;

public class CreateQueueLimitationCommandValidator : AbstractValidator<CreateQueueLimitationCommand>
{

    public CreateQueueLimitationCommandValidator()
    {
        RuleFor(v => v.QueueId)
            .NotNullWithMessage("صف");
    }

}




