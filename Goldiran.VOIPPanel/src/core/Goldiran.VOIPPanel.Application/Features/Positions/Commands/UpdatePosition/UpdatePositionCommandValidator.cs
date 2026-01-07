using FluentValidation;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.Application.Features.Positions.Commands.UpdatePosition;

public class UpdatePositionCommandValidator : AbstractValidator<UpdatePositionCommand>
{

    public UpdatePositionCommandValidator( )
    {
        RuleFor(v => v.Title)
            .NotNullWithMessage("عنوان")
            .NotEmptyWithMessage("عنوان")
            .MaximumLengthWithMessage(100, "عنوان");

        RuleFor(v => v.PositionTypeId)
            .NotNullWithMessage("نوع سمت سازمانی");

    }

}
