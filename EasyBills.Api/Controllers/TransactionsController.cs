using AutoMapper;
using EasyBills.Api.Authorization;
using EasyBills.Api.Models;
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
    /// The mapper used to transform an object into a different type.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionsController"/> class.
    /// </summary>
    /// <param name="transactionRepository">The transaction repository instance.</param>
    /// <param name="accountRepository">The account repository instance.</param>
    /// <param name="categoryRepository">The category repository instance.</param>
    /// <param name="mapper">Mapper instance.</param>
    public TransactionsController(ITransactionRepository transactionRepository, IAccountRepository accountRepository, ICategoryRepository categoryRepository, IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _categoryRepository = categoryRepository;
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
    public async Task<List<TransactionDTO>> GetTransactions()
    {
        var transactions = await _transactionRepository.GetAll(null, "Account,Category");

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
        var transaction = await _transactionRepository.GetById(id, "Account,Category");

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
        var transaction = await _transactionRepository.GetById(id);

        if (transaction is null)
        {
            return NotFound(new ErrorResponse { Error = "La transaccion no existe" });
        }

        _transactionRepository.Remove(transaction);
        await _transactionRepository.SaveChanges();

        return NoContent();
    }
}
