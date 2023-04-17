using AutoMapper;
using EasyBills.Application.Accounts;
using EasyBills.Application.Categories;
using EasyBills.Application.Transactions;
using EasyBills.Application.Users;
using EasyBills.Domain.Entities;

namespace EasyBills.Application.Tools;

/// <summary>
/// Auto mapper profiles used for mapping the entities.
/// </summary>
public class AutoMapperProfiles : Profile
{
    /// <summary>
    /// Create mapping configuration.
    /// </summary>
    public AutoMapperProfiles()
    {
        CreateMap<User, UserDTO>();
        CreateMap<CreateUserDTO, User>();
        CreateMap<Category, CategoryDTO>();
        CreateMap<CreateCategoryDTO, Category>();
        CreateMap<Account, AccountDTO>();
        CreateMap<CreateAccountDTO, Account>();
        CreateMap<Transaction, TransactionDTO>();
        CreateMap<CreateTransactionDTO, Transaction>();
    }
}
