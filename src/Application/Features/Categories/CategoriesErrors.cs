using BackendAuthTemplate.Application.Common.Result;

namespace BackendAuthTemplate.Application.Features.Categories
{
    public class CategoriesErrors : EntityError<CategoriesErrors>
    {
        protected override string Entity => "Category";
    }
}
