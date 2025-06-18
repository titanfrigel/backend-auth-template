using BackendAuthTemplate.Domain.Common;

namespace BackendAuthTemplate.Domain.Entities
{
    public class Subcategory : BaseAuditableEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
