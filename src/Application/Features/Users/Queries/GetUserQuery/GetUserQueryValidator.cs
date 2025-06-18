using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.Users.Queries.GetUserQuery
{
    public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserQueryValidator()
        {
            _ = RuleFor(x => x.UserId).NotEmpty().WithErrorCode(ValidationCodes.Required);
        }
    }
}
