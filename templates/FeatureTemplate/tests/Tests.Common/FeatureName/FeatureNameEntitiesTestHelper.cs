using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Domain.Entities;

namespace BackendAuthTemplate.Tests.Common.FeatureName
{
    public static class FeatureNameEntitiesTestHelper
    {
        public static EntityName CreateValidEntityName(
            string name = "Default EntityName"
        )
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = name
            };
        }

        public static async Task<EntityName> SeedEntityName(
            IAppDbContext context,
            EntityName? entity = null
        )
        {
            if (entity != null)
            {
                EntityName entityName = entity;

                _ = context.FeatureName.Add(entityName);
                _ = await context.SaveChangesAsync(CancellationToken.None);

                return entityName;

            }

            EntityName newEntityName = CreateValidEntityName();

            _ = context.FeatureName.Add(newEntityName);
            _ = await context.SaveChangesAsync(CancellationToken.None);

            return newEntityName;
        }
    }
}
