using AutoMapper;
using EasyBills.Application.Categories;
using EasyBills.Application.Users;
using EasyBills.Domain.Entities;

namespace EasyBills.Application.Tools;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<User, UserDTO>();
        CreateMap<CreateUserDTO, User>();
        CreateMap<Category, CategoryDTO>();
        CreateMap<CreateCategoryDTO, Category>();
    }
}
