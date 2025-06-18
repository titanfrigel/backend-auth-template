using BackendAuthTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendAuthTemplate.Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Category> Categories { get; set; }
        DbSet<Subcategory> Subcategories { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
