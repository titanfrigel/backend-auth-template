using BackendAuthTemplate.Domain.Common;

namespace BackendAuthTemplate.Domain.Entities
{
    public class EntityName : BaseAuditableEntity
    {
        public required string Name { get; set; }
    }
}
