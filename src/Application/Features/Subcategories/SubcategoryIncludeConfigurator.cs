using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Domain.Entities;

namespace BackendAuthTemplate.Application.Features.Subcategories;

public class SubcategoryIncludeConfigurator : IIncludeConfigurator<Subcategory>
{
    public IncludableProperties<Subcategory> Configure()
    {
        return new IncludableProperties<Subcategory>()
            .Add("Category")
            .Add("CreatedBy", isManualInclude: true, roles: ["Admin"]);
    }
}
