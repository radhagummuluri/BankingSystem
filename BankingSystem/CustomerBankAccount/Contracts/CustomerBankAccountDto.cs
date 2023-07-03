namespace BankingSystem.WebApi.CustomerBankAccount.Models
{
    public record CustomerDto(long Id, string Ssn, string FirstName, string LastName, IEnumerable<BankAccountDto> BankAccounts);
    public record BankAccountDto(long Id, decimal Amount);
}
