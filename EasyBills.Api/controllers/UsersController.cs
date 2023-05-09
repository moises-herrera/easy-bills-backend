using AutoMapper;
using EasyBills.Api.Authorization;
using EasyBills.Api.Models;
using EasyBills.Application.Users;
using EasyBills.Security.Helpers;
using EasyBills.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using EasyBills.Domain.Interfaces;

namespace EasyBills.Api.Controllers;

/// <summary>
/// The controller for all user actions.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    /// <summary>
    /// User repository to handle user actions.
    /// </summary>
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Object used to get the configuration from the appsettings.
    /// </summary>
    private readonly IConfiguration _configuration;

    /// <summary>
    /// The _mapper used to transform an object into a different type.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository instance.</param>
    /// <param name="configuration">The object to get the configuration from the appsettings.json</param>
    /// <param name="mapper">Mapper instance.</param>
    public UsersController(
        IUserRepository userRepository, 
        IConfiguration configuration, 
        IMapper mapper)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all users.
    /// </summary>
    /// <returns>A list of users.</returns>
    [ProducesResponseType(typeof(List<UserDTO>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [Authorization]
    [HttpGet]
    public async Task<List<UserDTO>> GetUsers()
    {
        var users = await _userRepository.GetAll();

        return _mapper.Map<List<UserDTO>>(users);
    }

    /// <summary>
    /// Get a user by id.
    /// </summary>
    /// <param name="id">User id.</param>
    /// <returns>The user.</returns>
    /// <response code="404">If the user does not exist.</response>s
    [ProducesResponseType(typeof(UserDTO), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [Authorization]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDTO>> GetUserById(Guid id)
    {
        var user = await _userRepository.GetById(id);

        if (user is null)
        {
            return NotFound(new ErrorResponse { Error = "El usuario no existe" });
        }

        return _mapper.Map<UserDTO>(user);
    }

    /// <summary>
    /// Create a user.
    /// </summary>
    /// <param name="createUserDTO">Object with user data.</param>
    /// <returns>No content response</returns>
    /// <response code="204">If the user was created.</response>
    /// <response code="400">If the email was already taken.</response>
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    [HttpPost("register")]
    public async Task<ActionResult> CreateUser(CreateUserDTO createUserDTO)
    {
        var existingUser = await _userRepository.GetOne(user => user.Email == createUserDTO.Email);

        if (existingUser is not null)
        {
            return BadRequest(
                new ErrorResponse { Error = "Ya existe una cuenta con ese email" }
                );
        }
 
        var user = _mapper.Map<User>(createUserDTO);
        user.Password = EncryptionHelper.Encrypt(user.Password);

        _userRepository.Add(user);
        await _userRepository.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Update a user by id.
    /// </summary>
    /// <param name="updateUserDTO">Object with user data.</param>
    /// <param name="id">User id.</param>
    /// <returns>No content response.</returns>
    /// <response code="204">If the user was updated.</response>
    /// <response code="404">If the user does not exist.</response>
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [Authorization]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateUser(CreateUserDTO updateUserDTO, Guid id)
    {
        var userId = Guid.Parse(Request.HttpContext.Items["UserId"].ToString());
        var currentUser = await _userRepository.GetById(userId);

        if (currentUser is not null && !currentUser.IsAdmin && userId != id)
        {
            return Unauthorized(new ErrorResponse { Error = "No tienes acceso" });
        }
        var existingUser = await _userRepository.GetById(id);

        if (existingUser is null)
        {
            return NotFound(new ErrorResponse { Error = "El usuario no existe" });
        }

        if (updateUserDTO.Email != existingUser.Email)
        {
            var isEmailTaken = await _userRepository.GetOne(user => user.Email == updateUserDTO.Email);

            if (isEmailTaken is not null)
            {
                return BadRequest(new ErrorResponse { Error = "Ya existe un usuario con ese email" });
            }
        }

        var user = _mapper.Map<User>(updateUserDTO);
        user.Id = id;
        user.IsEmailVerified = user.IsEmailVerified && updateUserDTO.Email == existingUser.Email;
        user.Password = 
            !string.IsNullOrWhiteSpace(user.Password) 
                ? EncryptionHelper.Encrypt(user.Password) 
                : existingUser.Password;

        await _userRepository.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Delete a user by id.
    /// </summary>
    /// <param name="id">User id.</param>
    /// <returns>No content response.</returns>
    /// <response code="204">If the user was deleted.</response>
    /// <response code="404">If the user does not exist.</response>
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [Authorization]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        var userId = Guid.Parse(Request.HttpContext.Items["UserId"].ToString());

        if (userId == id)
        {
            return BadRequest(new ErrorResponse { Error = "No puedes realizar esta acción" });
        }

        var currentUser = await _userRepository.GetById(userId);

        if (currentUser is not null && !currentUser.IsAdmin && userId != id)
        {
            return Unauthorized(new ErrorResponse { Error = "No tienes acceso" });
        }

        var user = await _userRepository.GetById(id);

        if (user is null)
        {
            return NotFound(new ErrorResponse { Error = "El usuario no existe" });
        }

        _userRepository.Remove(user);
        await _userRepository.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Login a user.
    /// </summary>
    /// <param name="loginUserDTO">User credentials.</param>
    /// <returns>Response with <see cref="LoginUserDTO"/> model.</returns>
    [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
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

            loginResponse.AccessToken = JwtHelper.CreateJWT(_configuration, user.Id.ToString(), user.FullName, user.Email, Constants.accessTokenLifeTimeInMinutes);
            loginResponse.User = _mapper.Map<UserDTO>(user);
        }
        catch (Exception ex)
        {
            var message = $"Error al iniciar sesion";
            return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse { Error = message, Exception = ex.Message });
        }
        
        return Ok(loginResponse);
    }

    /// <summary>
    /// Validate access token.
    /// </summary>
    /// <returns>Response with <see cref="LoginUserDTO"/> model.</returns>
    [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    [Authorization]
    [HttpGet("renew-token")]
    public async Task<ActionResult> ValidateAccessToken()
    {
        try
        {
            var userId = Guid.Parse(Request.HttpContext.Items["UserId"].ToString());

            var user = await _userRepository.GetById(userId);

            if (user is null)
            {
                return Unauthorized(new ErrorResponse { Error = "No tienes acceso" });
            }

            var token = JwtHelper.CreateJWT(_configuration, user.Id.ToString(), user.FullName, user.Email, Constants.accessTokenLifeTimeInMinutes);
            var response = new LoginResponse { User = _mapper.Map<UserDTO>(user), AccessToken = token };

            return Ok(response);
        }
        catch (Exception ex)
        {
            var message = $"Error al validar el token";
            return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse { Error = message, Exception = ex.Message });
        }
    }

    /// <summary>
    /// Confirm user email.
    /// </summary>
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    [Authorization]
    [HttpPost("verify-email")]
    public async Task<ActionResult> ConfirmUserEmail(string userId)
    {
        try
        {
            var user = await _userRepository.GetById(Guid.Parse(userId));

            if (user is null)
            {
                return BadRequest(new ErrorResponse { Error = "Usuario no encontrado" });
            }

            user.IsEmailVerified = true;
            _userRepository.Update(user);
            await _userRepository.SaveChanges();

            return Ok();
        }
        catch (Exception ex)
        {
            var message = $"Error al verificar el email";
            return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse { Error = message, Exception = ex.Message });
        }
    }
}
