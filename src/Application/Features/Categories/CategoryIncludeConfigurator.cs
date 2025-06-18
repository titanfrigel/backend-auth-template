using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.Categories
{
    public class CategoryIncludeConfigurator : IncludeConfiguratorBase<Category>
    {
        public override HashSet<string> DefaultIncludes =>
        [
            "subcategories"
        ];

        public override IQueryable<Category> ApplyIncludes(IQueryable<Category> query, IncludeTree includes)
        {
            if (includes.ContainsKey("subcategories"))
            {
                query = query.Include(o => o.Subcategories);
            }

            return query;
        }
    }
}
