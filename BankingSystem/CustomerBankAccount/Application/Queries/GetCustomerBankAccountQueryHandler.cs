using BankingSystem.WebApi.CustomerBankAccount.Adapters;
using BankingSystem.WebApi.CustomerBankAccount.Models;
using BankingSystem.WebApi.CustomerBankAccount.Repository;
using MediatR;

namespace BankingSystem.WebApi.CustomerBankAccount.Application.Queries
{
    public class GetCustomerBankAccountQueryHandler : IRequestHandler<GetCustomerBankAccountQuery, CustomerDto?>
    {
        private readonly ICustomerBankAccountRepository _customerBankAccountRepository;

        public GetCustomerBankAccountQueryHandler(ICustomerBankAccountRepository customerBankAccountRepository)
        {
            _customerBankAccountRepository = customerBankAccountRepository;
        }

        public async Task<CustomerDto?> Handle(GetCustomerBankAccountQuery request, CancellationToken cancellationToken)
        {
            var account = await _customerBankAccountRepository.GetCustomerBankAccount(request.CustomerId, request.BankAccountId, cancellationToken);
            return account != null ? account.ToDto() : null;
        }
    }
}
