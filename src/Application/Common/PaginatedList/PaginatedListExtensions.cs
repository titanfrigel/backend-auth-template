using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Common.PaginatedList
{
    public static class PaginatedListExtensions
    {
        public static Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize, CancellationToken cancellationToken = default) where TDestination : class
        {
            return PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize, cancellationToken);
        }
    }
}
