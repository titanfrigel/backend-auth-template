namespace BackendAuthTemplate.Application.Common.PaginatedList
{
    public interface IPaginable
    {
        int PageNumber { get; init; }
        int PageSize { get; init; }
    }
}
