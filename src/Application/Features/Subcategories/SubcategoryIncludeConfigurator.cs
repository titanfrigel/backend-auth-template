using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Features.Subcategories
{
    public class SubcategoryIncludeConfigurator : IncludeConfiguratorBase<Subcategory>
    {
        public override HashSet<string> DefaultIncludes =>
        [
            "category"
        ];

        public override Dictionary<string, HashSet<string>> RoleExtras => new()
        {
            {
                "Admin",
                [
                "createdBy"
                ]
            }
        };

        public override IQueryable<Subcategory> ApplyIncludes(IQueryable<Subcategory> query, IncludeTree includes)
        {
            if (includes.ContainsKey("category"))
            {
                query = query.Include(o => o.Category);
            }

            return query;
        }
    }
}
