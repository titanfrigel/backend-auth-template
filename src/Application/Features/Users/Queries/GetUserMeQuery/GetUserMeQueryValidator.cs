using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.Users.Queries.GetUserMeQuery
{
    public class GetUserMeQueryValidator : AbstractValidator<GetUserMeQuery>
    {
        public GetUserMeQueryValidator()
        {
            _ = RuleFor(x => x).NotNull().WithErrorCode(ValidationCodes.Required);
        }
    }
}
