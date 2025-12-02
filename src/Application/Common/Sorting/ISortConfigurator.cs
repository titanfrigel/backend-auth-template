namespace BackendAuthTemplate.Application.Common.Sorting
{
    public interface ISortConfigurator<T>
    {
        SortableProperties<T> Configure();
    }
}
