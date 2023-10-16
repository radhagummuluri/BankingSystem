using BankingSystem.WebApi.CustomerBankAccount.Models;

namespace BankingSystem.WebApi.CustomerBankAccount.Adapters
{
    public static class CustomerBankAccountAdapter
    {
        public static IEnumerable<BankAccountDto> ToDtos(this IEnumerable<Domain.BankAccount> customerBankAccounts) =>
            customerBankAccounts.Any() ? customerBankAccounts.Select(ba => ba.ToDto()) : Enumerable.Empty<BankAccountDto>();

        public static BankAccountDto ToDto(this Domain.BankAccount customerBankAccount) =>
            new BankAccountDto(customerBankAccount.Id, customerBankAccount.Amount);

        public static CustomerDto ToDto(this Domain.Customer customer) =>
            new CustomerDto(customer.Id, customer.Ssn, customer.FirstName, customer.LastName, customer.BankAccounts.ToDtos());
    }
}
