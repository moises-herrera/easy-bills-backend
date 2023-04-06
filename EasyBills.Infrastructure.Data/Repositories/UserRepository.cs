using EasyBills.Domain.Entities;
using EasyBills.Infrastructure.Data.Context;

namespace EasyBills.Infrastructure.Data.Repositories;

public class UserRepository : RepositoryBase<User>
{
    public UserRepository(ApplicationDbContext context)
        : base(context)
    {

    }
}
