using BackendAuthTemplate.Application.Features.Categories.Queries.GetAllCategoriesWithPaginationQuery;
using BackendAuthTemplate.Application.Features.Categories.Queries.GetCategoryByIdQuery;

namespace BackendAuthTemplate.Tests.Common.Categories
{
    public static class CategoriesQueriesTestHelper
    {
        public static GetCategoryByIdQuery GetCategoryByIdQuery(
            Guid? id = null,
            List<string>? include = null
        )
        {
            return new()
            {
                CategoryId = id ?? Guid.NewGuid(),
                Include = include ?? []
            };
        }


        public static GetAllCategoriesWithPaginationQuery GetAllCategoriesWithPaginationQuery(
            int pageNumber = 1,
            int pageSize = 10,
            List<string>? include = null
        )
        {
            return new()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Include = include ?? []
            };
        }
    }
}
