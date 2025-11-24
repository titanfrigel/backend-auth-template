using BackendAuthTemplate.Application.Features.FeatureName.Queries.GetAllFeatureNameWithPaginationQuery;
using BackendAuthTemplate.Application.Features.FeatureName.Queries.GetEntityNameByIdQuery;

namespace BackendAuthTemplate.Tests.Common.FeatureName
{
    public static class FeatureNameQueriesTestHelper
    {
        public static GetEntityNameByIdQuery GetEntityNameByIdQuery(
            Guid? id = null,
            List<string>? include = null
        )
        {
            return new()
            {
                EntityNameId = id ?? Guid.NewGuid(),
                Include = include ?? []
            };
        }


        public static GetAllFeatureNameWithPaginationQuery GetAllFeatureNameWithPaginationQuery(
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
