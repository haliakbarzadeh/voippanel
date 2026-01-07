using FluentValidation;
using Voip.Framework.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.Application.Features.Queus.Commands.AddUserToQueue;


public class AddUserToQueueCommandValidator : AbstractValidator<AddUserToQueueCommand>
{

    public AddUserToQueueCommandValidator()
    {
        RuleFor(v => v.QueueName)
            .NotNullWithMessage("نام صف");

        //RuleFor(v => v.DepartmentId)
        //    .NotNullWithMessage("نوع واحد سازمانی");
    }

}




