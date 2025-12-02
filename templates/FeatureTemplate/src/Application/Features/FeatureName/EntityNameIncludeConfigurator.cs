using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Domain.Entities;

namespace BackendAuthTemplate.Application.Features.FeatureName
{
    public class EntityNameIncludeConfigurator : IIncludeConfigurator<EntityName>
    {
        public IncludableProperties<EntityName> Configure()
        {
            return new IncludableProperties<EntityName>()
                .Add("CreatedBy", isManualInclude: true, roles: ["Admin"]);
        }
    }
}
