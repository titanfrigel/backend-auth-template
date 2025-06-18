using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Common.PaginatedList
{
    public class PaginatedList<T>
    {
        public required IReadOnlyCollection<T> Items { get; init; }
        public required int PageNumber { get; init; }
        public required int TotalPages { get; init; }
        public required int TotalCount { get; init; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            int count = await source.CountAsync(cancellationToken);

            List<T> items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            int totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginatedList<T>()
            {
                Items = items,
                PageNumber = pageNumber,
                TotalPages = totalPages,
                TotalCount = count
            };
        }
    }
}
