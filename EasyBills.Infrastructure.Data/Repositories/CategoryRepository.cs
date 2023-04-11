using EasyBills.Domain.Entities;
using EasyBills.Domain.Interfaces;
using EasyBills.Infrastructure.Data.Context;

namespace EasyBills.Infrastructure.Data.Repositories;

public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext dbContext) 
        : base(dbContext)
    {
    }
}
