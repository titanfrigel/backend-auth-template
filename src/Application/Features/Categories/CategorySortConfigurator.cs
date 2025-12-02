using BackendAuthTemplate.Application.Common.Sorting;
using BackendAuthTemplate.Domain.Entities;

namespace BackendAuthTemplate.Application.Features.Categories;

public class CategorySortConfigurator : ISortConfigurator<Category>
{
    public SortableProperties<Category> Configure()
    {
        return new SortableProperties<Category>()
            .Add("Name");
    }
}
