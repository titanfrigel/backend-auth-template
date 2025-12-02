using BackendAuthTemplate.Application.Common.Sorting;
using BackendAuthTemplate.Domain.Entities;

namespace BackendAuthTemplate.Application.Features.FeatureName
{
    public class EntityNameSortConfigurator : ISortConfigurator<EntityName>
    {
        public SortableProperties<EntityName> Configure()
        {
            return new SortableProperties<EntityName>()
                .Add("Name");
        }
    }
}
