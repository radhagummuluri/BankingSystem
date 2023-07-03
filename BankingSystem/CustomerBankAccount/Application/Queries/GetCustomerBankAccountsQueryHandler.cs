using BankingSystem.WebApi.CustomerBankAccount.Adapters;
using BankingSystem.WebApi.CustomerBankAccount.Models;
using BankingSystem.WebApi.CustomerBankAccount.Repository;
using MediatR;

namespace BankingSystem.WebApi.CustomerBankAccount.Application.Queries
{
    public class GetCustomerBankAccountsQueryHandler : IRequestHandler<GetCustomerBankAccountsQuery, CustomerDto?>
    {
        private readonly ICustomerBankAccountRepository _customerBankAccountRepository;

        public GetCustomerBankAccountsQueryHandler(ICustomerBankAccountRepository customerBankAccountRepository)
        {
            _customerBankAccountRepository = customerBankAccountRepository;
        }

        public async Task<CustomerDto?> Handle(GetCustomerBankAccountsQuery request, CancellationToken cancellationToken)
        {
            var account = await _customerBankAccountRepository.GetCustomerWithAllBankAccounts(request.CustomerId, cancellationToken);
            return account != null ? account.ToDto() : null;
        }
    }
}
