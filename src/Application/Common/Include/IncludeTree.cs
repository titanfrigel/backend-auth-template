using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace BackendAuthTemplate.Application.Common.Include
{
    public class IncludeTree
    {
        private readonly Dictionary<string, IncludeTree> _tree;

        private IncludeTree()
        {
            _tree = [];
        }

        private IncludeTree this[string key]
        {
            get => _tree[key];
            set => _tree[key] = value;
        }

        public bool ContainsKey(string key)
        {
            return _tree.ContainsKey(key.ToLower());
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out IncludeTree value)
        {
            return _tree.TryGetValue(key.ToLower(), out value);
        }

        public static IncludeTree FromPaths(IEnumerable<string> paths)
        {
            IncludeTree root = new();

            foreach (string path in paths)
            {
                IncludeTree current = root;

                string[] parts = path.Split('.');

                if (parts.Length > IncludeParser.MaxIncludeDepth)
                {
                    throw new ValidationException($"Include path exceeds maximum depth of {IncludeParser.MaxIncludeDepth}: {path}");
                }

                foreach (string part in parts)
                {
                    string lowerPart = part.ToLower();

                    if (!current.ContainsKey(lowerPart))
                    {
                        current[lowerPart] = new();
                    }

                    current = current[lowerPart];
                }
            }

            return root;
        }
    }
}