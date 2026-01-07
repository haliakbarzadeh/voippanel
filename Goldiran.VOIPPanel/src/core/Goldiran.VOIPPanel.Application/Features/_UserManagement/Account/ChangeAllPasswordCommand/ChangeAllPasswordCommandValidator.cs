using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Saramad.Core.ApplicationService.Common.ExtensionMethod;
using Saramad.Core.ApplicationService.Common.Interfaces;
using Saramad.Core.ApplicationService.Features.Accounts.Commands.CreateAccount;
using System.Text.RegularExpressions;

namespace Saramad.Core.ApplicationService.Features.Account.ChangeAllPasswordCommand;

public class ChangeAllPasswordCommandValidator : AbstractValidator<ChangeAllPasswordCommand>
{

    public ChangeAllPasswordCommandValidator()
    {



    }


}
