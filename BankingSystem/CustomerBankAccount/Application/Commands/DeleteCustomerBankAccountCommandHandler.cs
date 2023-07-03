using BankingSystem.Data;
using BankingSystem.WebApi.Common.Exceptions;
using BankingSystem.WebApi.CustomerBankAccount.Repository;
using MediatR;

namespace BankingSystem.WebApi.CustomerBankAccount.Application.Commands
{
    public class DeleteCustomerBankAccountCommandHandler : IRequestHandler<DeleteCustomerBankAccountCommand, bool>
    {
        private readonly ICustomerBankAccountRepository _customerBankAccountRepository;

        public DeleteCustomerBankAccountCommandHandler(ICustomerBankAccountRepository customerBankAccountRepository)
        {
            _customerBankAccountRepository = customerBankAccountRepository;
        }

        public async Task<bool> Handle(DeleteCustomerBankAccountCommand command, CancellationToken cancellationToken)
        {
            var customer = await _customerBankAccountRepository.GetCustomerBankAccount(command.CustomerId, command.BankAccountId, cancellationToken)
                ?? throw new DomainException($"There is no such customer for the given CustomerId {command.CustomerId}");

            if (!customer.BankAccounts.Any())
            {
                throw new DomainException($"There is no such Bank Account with Id {command.BankAccountId} for the customerId {command.CustomerId}");
            }

            var bankAccount = customer.BankAccounts.First();
            await _customerBankAccountRepository.DeleteBankAccount(bankAccount, cancellationToken);
            return true;
        }
    }
}
