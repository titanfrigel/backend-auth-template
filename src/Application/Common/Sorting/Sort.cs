namespace BackendAuthTemplate.Application.Common.Sorting
{
    public class Sort
    {
        public required string PropertyName { get; init; }
        public required SortDirection Direction { get; init; }
    }
}
