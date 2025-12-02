using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Domain.Entities;

namespace BackendAuthTemplate.Application.Features.Categories;

public class CategoryIncludeConfigurator : IIncludeConfigurator<Category>
{
    public IncludableProperties<Category> Configure()
    {
        return new IncludableProperties<Category>()
            .Add("Subcategories")
            .Add("CreatedBy", isManualInclude: true, roles: ["Admin"]);
    }
}
