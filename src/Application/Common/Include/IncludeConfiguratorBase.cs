using BackendAuthTemplate.Application.Common.Interfaces;

namespace BackendAuthTemplate.Application.Common.Include
{
    public abstract class IncludeConfiguratorBase<TEntity> : IIncludeConfigurator<TEntity>
    {
        public abstract HashSet<string> DefaultIncludes { get; }
        public virtual Dictionary<string, HashSet<string>> RoleExtras => [];

        public virtual HashSet<string> AllowedIncludesFor(IEnumerable<string> roles)
        {
            HashSet<string> set = new(DefaultIncludes, StringComparer.OrdinalIgnoreCase);

            foreach (string r in roles)
            {
                if (RoleExtras.TryGetValue(r, out HashSet<string>? extra))
                {
                    set.UnionWith(extra);
                }
            }

            return set;
        }

        public abstract IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> q, IncludeTree t);
    }

}
