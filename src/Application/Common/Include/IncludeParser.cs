using FluentValidation;

namespace BackendAuthTemplate.Application.Common.Include
{
    public static class IncludeParser
    {
        public const int MaxIncludeDepth = 3;

        public static IncludeTree Parse(IEnumerable<string> paths, HashSet<string> allowed)
        {
            HashSet<string> lowerAllowed = allowed
                .Select(x => x.ToLower())
                .ToHashSet();

            foreach (string path in paths)
            {
                if (!lowerAllowed.Contains(path.ToLower()))
                {
                    throw new ValidationException($"Invalid include path: {path}");
                }
            }

            return IncludeTree.FromPaths(paths);
        }
    }
}
