using BackendAuthTemplate.Application.Common.Result;

namespace BackendAuthTemplate.Application.Features.Subcategories
{
    public class SubcategoriesErrors : EntityError<SubcategoriesErrors>
    {
        protected override string Entity => "Subcategory";
    }
}
