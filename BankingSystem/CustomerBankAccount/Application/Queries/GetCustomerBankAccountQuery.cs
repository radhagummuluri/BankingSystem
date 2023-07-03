using BankingSystem.WebApi.Common.Constants;
using BankingSystem.WebApi.CustomerBankAccount.Models;
using FluentValidation;
using MediatR;

namespace BankingSystem.WebApi.CustomerBankAccount.Application.Queries
{
    public record GetCustomerBankAccountQuery(long CustomerId, long BankAccountId) : IRequest<CustomerDto?>;

    public class GetCustomerBankAccountQueryValidator : AbstractValidator<GetCustomerBankAccountQuery>
    {
        public GetCustomerBankAccountQueryValidator()
        {
            RuleFor(x => x.CustomerId).GreaterThan(0).WithMessage("CustomerId should be valid");
            RuleFor(x => x.BankAccountId).GreaterThan(0).WithMessage("BankAccountId should be valid");
        }
    }
}
