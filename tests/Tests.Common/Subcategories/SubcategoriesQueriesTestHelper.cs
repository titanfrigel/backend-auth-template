using BackendAuthTemplate.Application.Features.Subcategories.Queries.GetAllSubcategoriesWithPaginationQuery;
using BackendAuthTemplate.Application.Features.Subcategories.Queries.GetSubcategoryByIdQuery;

namespace BackendAuthTemplate.Tests.Common.Subcategories
{
    public static class SubcategoriesQueriesTestHelper
    {
        public static GetSubcategoryByIdQuery GetSubcategoryByIdQuery(
            Guid? id = null,
            List<string>? include = null
        )
        {
            return new()
            {
                SubcategoryId = id ?? Guid.NewGuid(),
                Include = include ?? []
            };
        }

        public static GetAllSubcategoriesWithPaginationQuery GetAllSubcategoriesWithPaginationQuery(
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
