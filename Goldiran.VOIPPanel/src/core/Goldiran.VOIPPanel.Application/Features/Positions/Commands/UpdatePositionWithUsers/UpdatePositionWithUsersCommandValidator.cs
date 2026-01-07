using FluentValidation;
using Voip.Framework.Common.Extensions;



namespace Goldiran.VOIPPanel.Application.Features.Positions.Commands.UpdatePositionWithUsers;

public class UpdatePositionWithUsersCommandValidator : AbstractValidator<UpdatePositionWithUsersCommand>
{

    public UpdatePositionWithUsersCommandValidator( )
    {
        RuleFor(v => v.Title)
            .NotNullWithMessage("عنوان")
            .NotEmptyWithMessage("عنوان")
            .MaximumLengthWithMessage(100, "عنوان");

        RuleFor(v => v.PositionTypeId)
            .NotNullWithMessage("نوع سمت سازمانی");

    }

}
