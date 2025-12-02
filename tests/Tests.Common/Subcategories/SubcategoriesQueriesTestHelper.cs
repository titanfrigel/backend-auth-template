using BackendAuthTemplate.Application.Common.Sorting;
using BackendAuthTemplate.Application.Features.Subcategories.Queries.GetSubcategoriesWithPaginationQuery;
using BackendAuthTemplate.Application.Features.Subcategories.Queries.GetSubcategoryByIdQuery;

namespace BackendAuthTemplate.Tests.Common.Subcategories
{
    public static class SubcategoriesQueriesTestHelper
    {
        public static GetSubcategoryByIdQuery GetSubcategoryByIdQuery(
            Guid? id = null,
            IList<string>? includes = null
        )
        {
            return new()
            {
                SubcategoryId = id ?? Guid.NewGuid(),
                Includes = includes ?? []
            };
        }

        public static GetSubcategoriesWithPaginationQuery GetSubcategoriesWithPaginationQuery(
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
