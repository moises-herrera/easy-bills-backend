using AutoMapper;
using EasyBills.Api.Authorization;
using EasyBills.Api.Models;
using EasyBills.Application.Accounts;
using EasyBills.Domain.Entities;
using EasyBills.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EasyBills.Api.Controllers;

/// <summary>
/// The controller for all accounts actions.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    /// <summary>
    /// Account repository to handle all the actions.
    /// </summary>
    private readonly IAccountRepository _accountRepository;

    /// <summary>
    /// User repository to handle all the user actions.
    /// </summary>
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// The mapper used to transform an object into a different type.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountsController"/> class.
    /// </summary>
    /// <param name="accountRepository">The account repository instance.</param>
    /// <param name="userRepository">The user repository instance.</param>
    /// <param name="mapper">Mapper instance.</param>
    public AccountsController(IAccountRepository accountRepository, IUserRepository userRepository, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all accounts.
    /// </summary>
    /// <returns>A list of accounts.</returns>
    [ProducesResponseType(typeof(List<AccountDTO>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [Authorization]
    [HttpGet]
    public async Task<List<AccountDTO>> GetAccounts()
    {
        var userId = Guid.Parse(Request.HttpContext.Items["UserId"].ToString());
        var isUserAdmin = await _userRepository.IsUserAdmin(userId);
        IEnumerable<Account> accounts;

        if (isUserAdmin)
        {
            accounts = await _accountRepository.GetAll();
        } 
        else
        {
            accounts = await _accountRepository.GetAll(a => a.UserId == userId);
        }

        return _mapper.Map<List<AccountDTO>>(accounts);
    }

    /// <summary>
    /// Get an account by id.
    /// </summary>
    /// <param name="id">Account id.</param>
    /// <returns>The account.</returns>
    /// <response code="404">If the account does not exist.</response>
    [ProducesResponseType(typeof(AccountDTO), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [Authorization]
    [HttpGet("{id:guid}")]
    public async Task<AccountDTO> GetAccountById(Guid id)
    {
        var userId = Guid.Parse(Request.HttpContext.Items["UserId"].ToString());
        var isUserAdmin = await _userRepository.IsUserAdmin(userId);

        var account = await _accountRepository.GetOne(a => a.Id == id && (a.UserId == userId || isUserAdmin));

        return _mapper.Map<AccountDTO>(account);
    }

    /// <summary>
    /// Create an account.
    /// </summary>
    /// <param name="createAccountDTO">Object with account data.</param>
    /// <returns>No content response</returns>
    /// <response code="204">If the account was created.</response>
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [Authorization]
    [HttpPost]
    public async Task<ActionResult> CreateAccount(CreateAccountDTO createAccountDTO)
    {
        var account = _mapper.Map<Account>(createAccountDTO);

        _accountRepository.Add(account);
        await _accountRepository.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Update an account by id.
    /// </summary>
    /// <param name="createAccountDTO">Object with account data.</param>
    /// <param name="id">Account id.</param>
    /// <returns>No content response</returns>
    /// <response code="204">If the account was updated.</response>
    /// <response code="400">If the account does not exist.</response>
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [Authorization]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateAccount(CreateAccountDTO createAccountDTO, Guid id)
    {
        var userId = Guid.Parse(Request.HttpContext.Items["UserId"].ToString());
        var isUserAdmin = await _userRepository.IsUserAdmin(userId);

        var existingAccount = await _accountRepository.GetOne(a => a.Id == id && (a.UserId == userId || isUserAdmin));

        if (existingAccount is null)
        {
            return NotFound(new ErrorResponse { Error = "La cuenta no existe" });
        }

        var account = _mapper.Map<Account>(createAccountDTO);
        account.Id = id;
        _accountRepository.Update(account);
        await _accountRepository.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Delete an account by id.
    /// </summary>
    /// <param name="id">Account id.</param>
    /// <returns>No content response</returns>
    /// <response code="204">If the account was deleted.</response>
    /// <response code="400">If the account does not exist.</response>
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [Authorization]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteAccount(Guid id)
    {
        var userId = Guid.Parse(Request.HttpContext.Items["UserId"].ToString());
        var isUserAdmin = await _userRepository.IsUserAdmin(userId);
        var account = await _accountRepository.GetOne(a => a.Id == id && (a.UserId == userId || isUserAdmin));

        if (account is null)
        {
            return NotFound(new ErrorResponse { Error = "La cuenta no existe" });
        }

        _accountRepository.Remove(account);
        await _accountRepository.SaveChanges();

        return NoContent();
    }
}
