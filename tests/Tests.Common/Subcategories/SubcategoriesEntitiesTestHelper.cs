using BackendAuthTemplate.Application.Common.Interfaces;
using BackendAuthTemplate.Domain.Entities;
using BackendAuthTemplate.Tests.Common.Categories;

namespace BackendAuthTemplate.Tests.Common.Subcategories
{
    public static class SubcategoriesEntitiesTestHelper
    {
        public static Subcategory CreateValidSubcategory(
            Guid? categoryId = null,
            string name = "Default Subcategory",
            string description = "Default Subcategory Description"
        )
        {
            return new()
            {
                Id = Guid.NewGuid(),
                CategoryId = categoryId ?? Guid.NewGuid(),
                Name = name,
                Description = description
            };
        }

        public static async Task<Subcategory> SeedSubcategory(
            IAppDbContext context,
            Subcategory? entity = null,
            Guid? categoryId = null
        )
        {
            if (entity != null)
            {
                if (categoryId.HasValue)
                {
                    entity.CategoryId = categoryId.Value;
                }

                _ = context.Subcategories.Add(entity);
                _ = await context.SaveChangesAsync(CancellationToken.None);

                return entity;
            }

            Guid catId = categoryId ?? (await CategoriesEntitiesTestHelper.SeedCategory(context)).Id;
            Subcategory s = CreateValidSubcategory(categoryId: catId);

            _ = context.Subcategories.Add(s);
            _ = await context.SaveChangesAsync(CancellationToken.None);

            return s;
        }
    }
}
