using BackendAuthTemplate.Application.Common.Sorting;
using BackendAuthTemplate.Application.Features.Categories.Queries.GetCategoriesWithPaginationQuery;
using BackendAuthTemplate.Application.Features.Categories.Queries.GetCategoryByIdQuery;

namespace BackendAuthTemplate.Tests.Common.Categories
{
    public static class CategoriesQueriesTestHelper
    {
        public static GetCategoryByIdQuery GetCategoryByIdQuery(
            Guid? id = null,
            IList<string>? includes = null
        )
        {
            return new()
            {
                CategoryId = id ?? Guid.NewGuid(),
                Includes = includes ?? []
            };
        }


        public static GetCategoriesWithPaginationQuery GetCategoriesWithPaginationQuery(
            int pageNumber = 1,
            int pageSize = 10,
            IList<string>? includes = null,
            IList<Sort>? sorts = null
        )
        {
            return new()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Includes = includes ?? [],
                Sorts = sorts ?? []
            };
        }
    }
}
