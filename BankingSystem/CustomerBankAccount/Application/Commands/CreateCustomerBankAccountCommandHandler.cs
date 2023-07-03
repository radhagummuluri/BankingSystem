using BankingSystem.WebApi.CustomerBankAccount.Contracts;
using BankingSystem.WebApi.CustomerBankAccount.Repository;
using MediatR;

namespace BankingSystem.WebApi.CustomerBankAccount.Application.Commands
{
    public class CreateCustomerBankAccountCommandHandler : IRequestHandler<CreateCustomerBankAccountCommand, CreatedBankAccountResponse>
    {
        private readonly ICustomerBankAccountRepository _customerBankAccountRepository;

        public CreateCustomerBankAccountCommandHandler(ICustomerBankAccountRepository customerBankAccountRepository)
        {
            _customerBankAccountRepository = customerBankAccountRepository;
        }

        public async Task<CreatedBankAccountResponse> Handle(CreateCustomerBankAccountCommand command, CancellationToken cancellationToken)
        {
            var account = new Domain.BankAccount { Amount = command.DepositAmount };
            var customer = await _customerBankAccountRepository.GetCustomer(command.Ssn, cancellationToken);
            
            if (customer != null)
            {
                customer.BankAccounts.Add(account);
                var updateResult = await _customerBankAccountRepository.SaveCustomer(customer, false, cancellationToken);
                return new CreatedBankAccountResponse(updateResult.Id, updateResult.BankAccounts.First().Id);
            }

            var newCustomer = new Domain.Customer
            {
                Ssn = command.Ssn,
                FirstName = command.FirstName,
                LastName = command.LastName,
            };
            newCustomer.BankAccounts.Add(account);
            var result = await _customerBankAccountRepository.SaveCustomer(newCustomer, true, cancellationToken) ;
            return new CreatedBankAccountResponse(result.Id, result.BankAccounts.First().Id);
        }
    }
}
