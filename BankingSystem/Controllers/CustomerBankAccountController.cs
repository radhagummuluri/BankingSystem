using BankingSystem.WebApi.CustomerBankAccount.Application.Commands;
using BankingSystem.WebApi.CustomerBankAccount.Application.Queries;
using BankingSystem.WebApi.CustomerBankAccount.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerBankAccountController : ControllerBase
    { 
        private readonly ISender _sender;

        public CustomerBankAccountController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Gets all bank accounts for a given customerId
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{customerId:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerDto>> GetAll(long customerId, CancellationToken cancellationToken)
        {
            var account = await _sender.Send(new GetCustomerBankAccountsQuery(customerId), cancellationToken);
            return account != null ? Ok(account) : NotFound();
        }

        /// <summary>
        /// Gets a bank account for a given customerId and bankAccountId
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="bankAccountId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{customerId:long}/bankAccount/{bankAccountId:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerDto>> GetById(long customerId, long bankAccountId, CancellationToken cancellationToken)
        {
            var account = await _sender.Send(new GetCustomerBankAccountQuery(customerId, bankAccountId), cancellationToken);
            return account?.BankAccounts.Any() ?? false ? Ok(account) : NotFound(); 
        }

        /// <summary>
        /// Creates a BankAccount for a given customer. Returns the customerId and bankAccountId in the response if successful.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<long>> Create([FromBody] CreateCustomerBankAccountCommand command, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(command, cancellationToken);
            return CreatedAtAction("Create", response);
        }

        /// <summary>
        /// Deposits the specified amount into the specified customer bank account.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("deposit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> Deposit([FromBody] DepositIntoCustomerBankAccountCommand command, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(command, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Withdraws the specified amount from the specified customer bank account.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("withdraw")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> Withdraw([FromBody] WithdrawFromCustomerBankAccountCommand command, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(command, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a bank account for a customer.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> Delete([FromBody] DeleteCustomerBankAccountCommand command, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}