using EasyBills.Domain.Entities;
using EasyBills.Domain.Interfaces;
using EasyBills.Infrastructure.Data.Context;

namespace EasyBills.Infrastructure.Data.Repositories;

/// <summary>
/// Category repository to manage category records.
/// </summary>
public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
{
    /// <summary>
    /// Initialize a new instance of <see cref="CategoryRepository"/> class.
    /// </summary>
    /// <param name="dbContext">App db context.</param>
    public CategoryRepository(ApplicationDbContext dbContext) 
        : base(dbContext)
    {
    }
}
