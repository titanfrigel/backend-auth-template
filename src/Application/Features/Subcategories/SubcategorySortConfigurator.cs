using BackendAuthTemplate.Application.Common.Sorting;
using BackendAuthTemplate.Domain.Entities;

namespace BackendAuthTemplate.Application.Features.Subcategories;

public class SubcategorySortConfigurator : ISortConfigurator<Subcategory>
{
    public SortableProperties<Subcategory> Configure()
    {
        return new SortableProperties<Subcategory>()
            .Add("Name")
            .Add("Category.Name");
    }
}
