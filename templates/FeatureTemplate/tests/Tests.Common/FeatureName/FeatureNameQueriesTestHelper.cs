using BackendAuthTemplate.Application.Common.Sorting;
using BackendAuthTemplate.Application.Features.FeatureName.Queries.GetFeatureNameWithPaginationQuery;
using BackendAuthTemplate.Application.Features.FeatureName.Queries.GetEntityNameByIdQuery;

namespace BackendAuthTemplate.Tests.Common.FeatureName
{
    public static class FeatureNameQueriesTestHelper
    {
        public static GetEntityNameByIdQuery GetEntityNameByIdQuery(
            Guid? id = null,
            IList<string>? includes = null
        )
        {
            return new()
            {
                EntityNameId = id ?? Guid.NewGuid(),
                Includes = includes ?? []
            };
        }


        public static GetFeatureNameWithPaginationQuery GetFeatureNameWithPaginationQuery(
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
