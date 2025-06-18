using BackendAuthTemplate.Domain.Common;

namespace BackendAuthTemplate.Domain.Entities
{
    public class Category : BaseAuditableEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }

        public ICollection<Subcategory> Subcategories { get; } = [];
    }
}
