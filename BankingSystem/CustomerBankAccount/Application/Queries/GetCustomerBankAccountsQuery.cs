using BankingSystem.WebApi.Common.Constants;
using BankingSystem.WebApi.CustomerBankAccount.Models;
using FluentValidation;
using MediatR;

namespace BankingSystem.WebApi.CustomerBankAccount.Application.Queries
{
    public record GetCustomerBankAccountsQuery(long CustomerId) : IRequest<CustomerDto?>;

    public class GetCustomerBankAccountsQueryValidator : AbstractValidator<GetCustomerBankAccountsQuery>
    {
        public GetCustomerBankAccountsQueryValidator()
        {
            RuleFor(x => x.CustomerId).GreaterThan(0).WithMessage("CustomerId should be valid");
        }
    }
}
