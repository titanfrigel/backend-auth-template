using BackendAuthTemplate.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Common.Include
{
    public static class IncludeExtensions
    {
        public static IQueryable<T> ApplyIncludes<T>(
            this IQueryable<T> query,
            IIncludable includable,
            IIncludeConfigurator<T> configurator,
            IUser user,
            out HashSet<string> validIncludes
        ) where T : class
        {
            if (includable.Includes == null || includable.Includes.Count == 0)
            {
                validIncludes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                return query;
            }

            IncludableProperties<T> includableProperties = configurator.Configure();
            validIncludes = includableProperties.GetValidIncludes(includable.Includes, user.Roles);

            foreach (string path in validIncludes)
            {
                if (!includableProperties.IsManualInclude(path))
                {
                    query = query.Include(path);
                }
            }

            return query;
        }
    }
}
