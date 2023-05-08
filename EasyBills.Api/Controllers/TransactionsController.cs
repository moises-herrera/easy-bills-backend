using AutoMapper;
using EasyBills.Api.Authorization;
using EasyBills.Api.Models;
using EasyBills.Application.Categories;
using EasyBills.Application.Transactions;
using EasyBills.Domain.Entities;
using EasyBills.Domain.Entities.Enums;
using EasyBills.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EasyBills.Api.Controllers;

/// <summary>
/// The controller for all actions abount transactions.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    /// <summary>
    /// The transation repository to handle transation actions.
    /// </summary>
    private readonly ITransactionRepository _transactionRepository;

    /// <summary>
    /// The account repository to handle account actions.
    /// </summary>
    private readonly IAccountRepository _accountRepository;

    /// <summary>
    /// The category repository to handle category actions.
    /// </summary>
    private readonly ICategoryRepository _categoryRepository;

    /// <summary>
    /// User repository to handle all the user actions.
    /// </summary>
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// The mapper used to transform an object into a different type.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionsController"/> class.
    /// </summary>
    /// <param name="transactionRepository">The transaction repository instance.</param>
    /// <param name="accountRepository">The account repository instance.</param>
    /// <param name="categoryRepository">The category repository instance.</param>
    /// <param name="userRepository">The user repository instance.</param>
    /// <param name="mapper">Mapper instance.</param>
    public TransactionsController(
        ITransactionRepository transactionRepository, 
        IAccountRepository accountRepository, 
        ICategoryRepository categoryRepository,
        IUserRepository userRepository,
        IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _categoryRepository = categoryRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all the transactions.
    /// </summary>
    /// <returns>A list of transactions.</returns>
    [ProducesResponseType(typeof(List<TransactionDTO>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [Authorization]
    [HttpGet]
    public async Task<List<TransactionDTO>> GetTransactions(string from = "", string to = "", int limit = 0)
    {
        var userId = Guid.Parse(Request.HttpContext.Items["UserId"].ToString());
        var isUserAdmin = await _userRepository.IsUserAdmin(userId);
        IEnumerable<Transaction> transactions;

        if (isUserAdmin)
        {
            transactions = await _transactionRepository.GetAll(null, "Account,Category");
        }
        else
        {
            transactions = await _transactionRepository.GetAll(t => t.Account.UserId == userId, "Account,Category");
        }

        if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
        {
            transactions = transactions.Where(t => t.CreatedDate >= DateTime.Parse(from).Date && t.CreatedDate <= DateTime.Parse(to)).ToList();
        }

        if (limit > 0)
        {
            transactions = transactions.Take(limit).ToList();
        }

        return _mapper.Map<List<TransactionDTO>>(transactions);
    }

    /// <summary>
    /// Get a transaction by id.
    /// </summary>
    /// <param name="id">Transaction id.</param>
    /// <returns>The transaction.</returns>
    /// <response code="404">If the transaction does not exist.</response>
    [ProducesResponseType(typeof(TransactionDTO), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [Authorization]
    [HttpGet("{id:guid}")]
    public async Task<TransactionDTO> GetTransactionById(Guid id)
    {
        var userId = Guid.Parse(Request.HttpContext.Items["UserId"].ToString());
        var isUserAdmin = await _userRepository.IsUserAdmin(userId);

        var transaction = await _transactionRepository.GetOne(t => t.Id == id && (t.Account.UserId == userId || isUserAdmin), "Account,Category");

        return _mapper.Map<TransactionDTO>(transaction);
    }

    /// <summary>
    /// Create a transaction.
    /// </summary>
    /// <param name="createTransactionDTO">Object with transaction data.</param>
    /// <returns>No content response</returns>
    /// <response code="204">If the transaction was created.</response>
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [Authorization]
    [HttpPost]
    public async Task<ActionResult> CreateTransaction(CreateTransactionDTO createTransactionDTO)
    {
        var account = await _accountRepository.GetById(createTransactionDTO.AccountId);

        if (account is null)
        {
            return BadRequest(new ErrorResponse { Error = "La cuenta no existe." });
        }

        var category = await _categoryRepository.GetById(createTransactionDTO.CategoryId);

        if (category is null)
        {
            return BadRequest(new ErrorResponse { Error = "La categoria no existe." });
        }

        var isIncome = createTransactionDTO.TransactionType == TransactionType.Income;

        account.Balance += isIncome ? createTransactionDTO.Amount : -createTransactionDTO.Amount;
        _accountRepository.Update(account);
        await _accountRepository.SaveChanges();

        var transaction = _mapper.Map<Transaction>(createTransactionDTO);

        _transactionRepository.Add(transaction);
        await _transactionRepository.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Update a transaction by id.
    /// </summary>
    /// <param name="createTransactionDTO">Object with transaction data.</param>
    /// <param name="id">Transaction id.</param>
    /// <returns>No content response</returns>
    /// <response code="204">If the transaction was updated.</response>
    /// <response code="400">If the transaction does not exist.</response>
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [Authorization]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateTransaction(CreateTransactionDTO createTransactionDTO, Guid id)
    {
        var userId = Guid.Parse(Request.HttpContext.Items["UserId"].ToString());
        var isUserAdmin = await _userRepository.IsUserAdmin(userId);

        var existingTransaction = await _transactionRepository.GetOne(t => t.Id == id && (t.Account.UserId == userId || isUserAdmin));

        if (existingTransaction is null)
        {
            return NotFound(new ErrorResponse { Error = "La transaccion no existe" });
        }

        var account = await _accountRepository.GetById(createTransactionDTO.AccountId);

        if (account is null)
        {
            return BadRequest(new ErrorResponse { Error = "La cuenta no existe." });
        }

        var isIncome = createTransactionDTO.TransactionType == TransactionType.Income;

        account.Balance += isIncome ? createTransactionDTO.Amount : -createTransactionDTO.Amount;
        _accountRepository.Update(account);
        await _accountRepository.SaveChanges();

        var transaction = _mapper.Map<Transaction>(createTransactionDTO);
        transaction.Id = id;

        _transactionRepository.Update(transaction);
        await _transactionRepository.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Delete a transaction by id.
    /// </summary>
    /// <param name="id">Transaction id.</param>
    /// <returns>No content response</returns>
    /// <response code="204">If the transaction was deleted.</response>
    /// <response code="400">If the transaction does not exist.</response>
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [Authorization]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteTransaction(Guid id)
    {
        var userId = Guid.Parse(Request.HttpContext.Items["UserId"].ToString());
        var isUserAdmin = await _userRepository.IsUserAdmin(userId);

        var existingTransaction = await _transactionRepository.GetOne(t => t.Id == id && (t.Account.UserId == userId || isUserAdmin));

        if (existingTransaction is null)
        {
            return NotFound(new ErrorResponse { Error = "La transaccion no existe" });
        }

        _transactionRepository.Remove(existingTransaction);
        await _transactionRepository.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Get all the transactions grouped by category.
    /// </summary>
    /// <returns>Groups of transactions.</returns>
    [ProducesResponseType(typeof(List<IGrouping<CategoryDTO, TransactionDTO>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [Authorization]
    [HttpGet("group/category")]
    public async Task<List<IGrouping<CategoryDTO, TransactionDTO>>> GetTransactionsByCategory(TransactionType transactionType = TransactionType.Spending, string from = "", string to = "")
    {
        var userId = Guid.Parse(Request.HttpContext.Items["UserId"].ToString());
        var isUserAdmin = await _userRepository.IsUserAdmin(userId);
        var transactions = await _transactionRepository.GetAll(t => (t.Account.UserId == userId || isUserAdmin) && t.TransactionType == transactionType, "Category");

        if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
        {
            transactions = transactions.Where(t => t.CreatedDate >= DateTime.Parse(from).Date && t.CreatedDate <= DateTime.Parse(to));
        }

        var transactionsDTO = _mapper.Map<List<TransactionDTO>>(transactions);
        var transactionsGroupedByCategory = transactionsDTO.GroupBy(t => t.Category).ToList();

        return transactionsGroupedByCategory;
    }
}
