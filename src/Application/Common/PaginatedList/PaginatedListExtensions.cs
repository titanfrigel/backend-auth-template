using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Common.PaginatedList
{
    public static class PaginatedListExtensions
    {
        public static Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize, CancellationToken cancellationToken = default) where TDestination : class
        {
            return PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize, cancellationToken);
        }

        public static PaginatedList<TDestination> MapPaginated<TDestination, TSource>(this IMapper mapper, PaginatedList<TSource> source, Func<TSource, Action<IMappingOperationOptions>>? opts = null)
        {
            List<TDestination> items = new(source.Items.Count);

            foreach (TSource item in source.Items)
            {
                Action<IMappingOperationOptions>? mappingOptions = opts?.Invoke(item);

                TDestination mapped = mappingOptions == null
                    ? mapper.Map<TDestination>(item)
                    : mapper.Map<TDestination>(item, mappingOptions);

                items.Add(mapped);
            }

            return new PaginatedList<TDestination>
            {
                PageNumber = source.PageNumber,
                TotalPages = source.TotalPages,
                TotalCount = source.TotalCount,
                Items = items
            };
        }
    }
}
