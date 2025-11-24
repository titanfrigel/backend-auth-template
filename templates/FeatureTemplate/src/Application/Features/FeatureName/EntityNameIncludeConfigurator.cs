using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Domain.Entities;

namespace BackendAuthTemplate.Application.Features.FeatureName
{
    public class EntityNameConfigurator : IncludeConfiguratorBase<EntityName>
    {
        public override HashSet<string> DefaultIncludes =>
        [
        ];

        public override Dictionary<string, HashSet<string>> RoleExtras => new()
        {
            {
                "Admin",
                [
                    "createdBy"
                ]
            }
        };

        public override IQueryable<EntityName> ApplyIncludes(IQueryable<EntityName> query, IncludeTree includes)
        {
            return query;
        }
    }
}
