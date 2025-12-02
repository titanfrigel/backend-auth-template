using BackendAuthTemplate.Application.Common.Interfaces;

namespace BackendAuthTemplate.Application.Common.Sorting
{
    public class SortableProperties<T> : IPropertiesConfigurator
    {
        public HashSet<string> Properties { get; } = new(StringComparer.OrdinalIgnoreCase);

        public SortableProperties<T> Add(string path)
        {
            _ = Properties.Add(path);

            return this;
        }

        public IEnumerable<string> GetDescriptiveProperties()
        {
            return Properties;
        }

        public List<Sort> GetValidSorts(IEnumerable<Sort> sorts)
        {
            List<Sort> validSorts = [];

            foreach (Sort sort in sorts)
            {
                if (Properties.TryGetValue(sort.PropertyName, out string? validName))
                {
                    validSorts.Add(new Sort
                    {
                        PropertyName = validName,
                        Direction = sort.Direction
                    });
                }
            }

            return validSorts;
        }
    }
}
