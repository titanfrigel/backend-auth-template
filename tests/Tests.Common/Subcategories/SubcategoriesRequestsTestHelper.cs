using BackendAuthTemplate.API.Requests.Subcategories;

namespace BackendAuthTemplate.Tests.Common.Subcategories
{
    public class SubcategoriesRequestsTestHelper
    {
        public static CreateSubcategoryRequest CreateSubcategoryRequest(
            string name = "FakeCategoryName",
            string description = "FakeCategoryDescription",
            Guid? categoryId = null
        )
        {
            return new()
            {
                Name = name,
                Description = description,
                CategoryId = categoryId ?? Guid.NewGuid()
            };
        }

        public static UpdateSubcategoryRequest UpdateSubcategoryRequest(
            string name = "FakeCategoryName",
            string description = "FakeCategoryDescription",
            Guid? categoryId = null
        )
        {
            return new()
            {
                Name = name,
                Description = description,
                CategoryId = categoryId ?? Guid.NewGuid()
            };
        }
    }
}
