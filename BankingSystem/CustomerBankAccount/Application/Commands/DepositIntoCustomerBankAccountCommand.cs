using FluentValidation;
using MediatR;

namespace BankingSystem.WebApi.CustomerBankAccount.Application.Commands
{
    public record DepositIntoCustomerBankAccountCommand(long CustomerId, long BankAccountId, decimal Amount) : IRequest<bool>;

    public class DepositIntoCustomerBankAccountCommandValidator : AbstractValidator<DepositIntoCustomerBankAccountCommand>
    {
        public DepositIntoCustomerBankAccountCommandValidator()
        {
            RuleFor(x => x.CustomerId).GreaterThan(0).WithMessage("CustomerId value is invalid");
            RuleFor(x => x.BankAccountId).GreaterThan(0).WithMessage("BankAccountId value is invalid");
            RuleFor(x => x.Amount)
                .LessThanOrEqualTo(10000)
                .WithMessage("Amount cannot be greater than $10,000")
                .GreaterThanOrEqualTo(100)
                .WithMessage("Amount cannot be less than $100");
        }
    }
}
