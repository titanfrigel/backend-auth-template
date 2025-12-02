namespace BackendAuthTemplate.Application.Common.Sorting
{
    public interface ISortable
    {
        IList<Sort>? Sorts { get; init; }
    }
}
