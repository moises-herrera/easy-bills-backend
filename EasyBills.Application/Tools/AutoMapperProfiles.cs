using AutoMapper;
using EasyBills.Domain.Entities;
using EasyBills.Domain.User;
using EasyBills.Domain.Users;

namespace EasyBills.Application.Tools;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<User, UserDTO>();
        CreateMap<CreateUserDTO, User>();
    }
}
