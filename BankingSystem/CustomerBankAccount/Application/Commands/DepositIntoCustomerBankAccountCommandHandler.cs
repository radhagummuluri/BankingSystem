using BankingSystem.WebApi.Common.Exceptions;
using BankingSystem.WebApi.CustomerBankAccount.Repository;
using MediatR;

namespace BankingSystem.WebApi.CustomerBankAccount.Application.Commands
{
    public class DepositIntoCustomerBankAccountCommandHandler : IRequestHandler<DepositIntoCustomerBankAccountCommand, bool>
    {
        private readonly ICustomerBankAccountRepository _customerBankAccountRepository;

        public DepositIntoCustomerBankAccountCommandHandler(ICustomerBankAccountRepository customerBankAccountRepository)
        {
            _customerBankAccountRepository = customerBankAccountRepository;
        }

        public async Task<bool> Handle(DepositIntoCustomerBankAccountCommand command, CancellationToken cancellationToken)
        {
            var customer = await _customerBankAccountRepository.GetCustomerBankAccount(command.CustomerId, command.BankAccountId, cancellationToken)
                ?? throw new DomainException($"There is no such customer for the given CustomerId {command.CustomerId}");

            if(!customer.BankAccounts.Any())
            {
                throw new DomainException($"There is no such Bank Account with Id {command.BankAccountId} for the customerId {command.CustomerId}");
            }

            var bankAccount = customer.BankAccounts.First();
            bankAccount.Amount += command.Amount;
            await _customerBankAccountRepository.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
