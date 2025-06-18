using BackendAuthTemplate.Application.Common.Interfaces;

namespace BackendAuthTemplate.Application.Common.Include
{
    public class IncludeBuilder<TEntity>(IIncludeConfigurator<TEntity> configurator, IEnumerable<string> paths, IEnumerable<string> roles)
    {
        public readonly IncludeTree tree = IncludeParser.Parse(paths, configurator.AllowedIncludesFor(roles));

        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            return configurator.ApplyIncludes(query, tree);
        }
    }
}
