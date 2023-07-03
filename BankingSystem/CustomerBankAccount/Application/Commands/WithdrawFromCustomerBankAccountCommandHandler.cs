using BankingSystem.WebApi.Common.Exceptions;
using BankingSystem.WebApi.CustomerBankAccount.Repository;
using MediatR;

namespace BankingSystem.WebApi.CustomerBankAccount.Application.Commands
{
    public class WithdrawFromCustomerBankAccountCommandHandler : IRequestHandler<WithdrawFromCustomerBankAccountCommand, bool>
    {
        private readonly ICustomerBankAccountRepository _customerBankAccountRepository;

        public WithdrawFromCustomerBankAccountCommandHandler(ICustomerBankAccountRepository customerBankAccountRepository)
        {
            _customerBankAccountRepository = customerBankAccountRepository;
        }

        public async Task<bool> Handle(WithdrawFromCustomerBankAccountCommand command, CancellationToken cancellationToken)
        {
            var customer = await _customerBankAccountRepository.GetCustomerBankAccount(command.CustomerId, command.BankAccountId, cancellationToken)
                ?? throw new DomainException($"There is no such customer for the given CustomerId {command.CustomerId}");

            if (!customer.BankAccounts.Any())
            {
                throw new DomainException($"There is no such Bank Account with Id {command.BankAccountId} for the customerId {command.CustomerId}");
            }

            var bankAccount = customer.BankAccounts.First();

            if (command.Amount * 100 / bankAccount.Amount > 90)
            {
                throw new DomainException($"Cannot withdraw more than 90% of total balance from an account in a single transaction");
            }

            bankAccount.Amount -= command.Amount;
            await _customerBankAccountRepository.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
