using BackendAuthTemplate.Application.Common.Include;

namespace BackendAuthTemplate.Application.Common.Interfaces
{
    public interface IIncludeConfigurator<TEntity>
    {
        HashSet<string> DefaultIncludes { get; }
        Dictionary<string, HashSet<string>> RoleExtras { get; }
        HashSet<string> AllowedIncludesFor(IEnumerable<string> roles);
        IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, IncludeTree includes);
    }
}
