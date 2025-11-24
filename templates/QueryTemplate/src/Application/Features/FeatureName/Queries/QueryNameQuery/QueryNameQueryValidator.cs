using BackendAuthTemplate.Application.Common.Validation;
using FluentValidation;

namespace BackendAuthTemplate.Application.Features.FeatureName.Queries.QueryNameQuery
{
    public class QueryNameQueryValidator : AbstractValidator<QueryNameQuery>
    {
        public QueryNameQueryValidator()
        {
            _ = RuleFor(x => x.EntityNameId).NotEmpty().WithErrorCode(ValidationCodes.Required);
        }
    }
}
