using BackendAuthTemplate.Application.Common.Interfaces;

namespace BackendAuthTemplate.Application.Common.Include
{
    public class IncludableProperties<T> : IPropertiesConfigurator
    {
        private readonly Dictionary<string, List<string>> _roleBasedProperties = [];
        private readonly HashSet<string> _properties = new(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, (bool IsManualInclude, List<string>? Roles)> AllProperties { get; } = new(StringComparer.OrdinalIgnoreCase);

        public IncludableProperties<T> Add(string path, bool isManualInclude = false, List<string>? roles = null)
        {
            AllProperties[path] = (isManualInclude, roles);

            if (roles != null)
            {
                foreach (string role in roles)
                {
                    if (!_roleBasedProperties.TryGetValue(role, out List<string>? value))
                    {
                        value = [];
                        _roleBasedProperties[role] = value;
                    }

                    value.Add(path);
                }
            }
            else
            {
                _ = _properties.Add(path);
            }

            return this;
        }

        public IEnumerable<string> GetDescriptiveProperties()
        {
            return AllProperties.Select(p =>
            {
                string roles = p.Value.Roles != null ? $" (Roles: {string.Join(", ", p.Value.Roles)})" : "";

                return $"{p.Key}{roles}";
            });
        }

        public bool IsManualInclude(string path)
        {
            if (AllProperties.TryGetValue(path, out (bool IsManualInclude, List<string>? Roles) value))
            {
                return value.IsManualInclude;
            }

            return false;
        }

        public HashSet<string> GetValidIncludes(IEnumerable<string> includables, IEnumerable<string> roles)
        {
            HashSet<string> allowedIncludes = new(_properties, StringComparer.OrdinalIgnoreCase);

            foreach (string role in roles)
            {
                if (_roleBasedProperties.TryGetValue(role, out List<string>? properties))
                {
                    foreach (string property in properties)
                    {
                        _ = allowedIncludes.Add(property);
                    }
                }
            }

            HashSet<string> requestedIncludes = new(includables, StringComparer.OrdinalIgnoreCase);

            return allowedIncludes.Where(requestedIncludes.Contains).ToHashSet(StringComparer.OrdinalIgnoreCase);
        }
    }
}
