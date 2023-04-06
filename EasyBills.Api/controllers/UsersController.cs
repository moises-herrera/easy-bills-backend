using AutoMapper;
using EasyBills.Api.Authorization;
using EasyBills.Api.Models;
using EasyBills.Application.Users;
using EasyBills.Security.Helpers;
using EasyBills.Domain.Entities;
using EasyBills.Domain.User;
using EasyBills.Domain.Users;
using EasyBills.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EasyBills.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IRepositoryBase<User> _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public UsersController(
        IRepositoryBase<User> userRepository, 
        IConfiguration configuration, 
        IMapper mapper)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _mapper = mapper;
    }

    [Authorization]
    [HttpGet]
    public async Task<List<UserDTO>> GetUsers()
    {
        var users = await _userRepository.GetAll();

        return _mapper.Map<List<UserDTO>>(users);
    }

    [Authorization]
    [HttpGet("{id:guid}")]
    public async Task<UserDTO> GetUserById(Guid id)
    {
        var user = await _userRepository.GetById(id);

        return _mapper.Map<UserDTO>(user);
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser(CreateUserDTO createUserDTO)
    {
        var user = _mapper.Map<User>(createUserDTO);

        _userRepository.Add(user);
        await _userRepository.SaveChanges();

        return NoContent();
    }

    [Authorization]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateUser(CreateUserDTO updateUserDTO, Guid id)
    {
        var user = _mapper.Map<User>(updateUserDTO);
        user.Id = id;
        _userRepository.Update(user);
        await _userRepository.SaveChanges();

        return Ok();
    }

    [Authorization]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        var user = await _userRepository.GetById(id);

        if (user is null)
        {
            return NotFound();
        }

        _userRepository.Remove(user);
        await _userRepository.SaveChanges();

        return Ok();
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginUserDTO loginUserDTO)
    {
        var loginResponse = new LoginResponse();

        try
        {
            var user = await _userRepository.GetOne(user => user.Email == loginUserDTO.Email);

            if (user is null || !user.IsValidPassword(loginUserDTO.Password))
            {
                var message = "Usuario o contraseña invalidos";
                return StatusCode((int)HttpStatusCode.Unauthorized, new ErrorResponse { Error = message });
            }

            if (!user.IsEmailVerified)
            {
                var message = "Tu cuenta de usuario no ha sido verificada";
                return StatusCode((int)HttpStatusCode.Unauthorized, new ErrorResponse { Error = message });
            }

            loginResponse.AccessToken = JwtHelper.CreateJWT(_configuration, user.Id.ToString(), user.FullName, user.Email, Constants.tokenLifeTimeInMinutes);
            loginResponse.User = _mapper.Map<UserDTO>(user);
        }
        catch (Exception ex)
        {
            var message = $"Error al iniciar sesion: {ex.Message}";
            return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse { Error = message });
        }

        return Ok(loginResponse);
    }
}
