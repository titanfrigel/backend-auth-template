using BackendAuthTemplate.API.Requests.Categories;

namespace BackendAuthTemplate.Tests.Common.Categories
{
    public class CategoriesRequestsTestHelper
    {
        public static CreateCategoryRequest CreateCategoryRequest(
            string name = "FakeCategoryName",
            string description = "FakeCategoryDescription"
        )
        {
            return new()
            {
                Name = name,
                Description = description
            };
        }

        public static UpdateCategoryRequest UpdateCategoryRequest(
            string name = "FakeCategoryName",
            string description = "FakeCategoryDescription"
        )
        {
            return new()
            {
                Name = name,
                Description = description
            };
        }
    }
}
