using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Domain.Entities;

namespace BackendAuthTemplate.Tests.Common.Categories
{
    public static class CategoriesEntitiesTestHelper
    {
        public static Category CreateValidCategory(
            string name = "Default Category",
            string description = "Default Category Description"
        )
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description
            };
        }

        public static async Task<Category> SeedCategory(
            IAppDbContext context,
            Category? entity = null
        )
        {
            if (entity != null)
            {
                Category cat = entity;

                _ = context.Categories.Add(cat);
                _ = await context.SaveChangesAsync(CancellationToken.None);

                return cat;

            }

            Category c = CreateValidCategory();

            _ = context.Categories.Add(c);
            _ = await context.SaveChangesAsync(CancellationToken.None);

            return c;
        }
    }
}
