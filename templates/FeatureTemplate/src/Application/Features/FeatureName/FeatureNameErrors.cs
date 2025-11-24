using BackendAuthTemplate.Application.Common.Result;

namespace BackendAuthTemplate.Application.Features.FeatureName
{
    public class FeatureNameErrors : EntityError<FeatureNameErrors>
    {
        protected override string Entity => "EntityName";
    }
}
