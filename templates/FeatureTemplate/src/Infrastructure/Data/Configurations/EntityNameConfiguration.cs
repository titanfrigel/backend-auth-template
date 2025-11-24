using BackendAuthTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendAuthTemplate.Infrastructure.Data.Configurations
{
    public class EntityNameConfiguration : IEntityTypeConfiguration<EntityName>
    {
        public void Configure(EntityTypeBuilder<EntityName> builder)
        {
        }
    }
}