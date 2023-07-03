using FluentValidation;
using MediatR;

namespace BankingSystem.WebApi.CustomerBankAccount.Application.Commands
{
    public record WithdrawFromCustomerBankAccountCommand(long CustomerId, long BankAccountId, decimal Amount) : IRequest<bool>;

    public class WithdrawFromCustomerBankAccountCommandValidator : AbstractValidator<WithdrawFromCustomerBankAccountCommand>
    {
        public WithdrawFromCustomerBankAccountCommandValidator()
        {
            RuleFor(x => x.CustomerId).GreaterThan(0).WithMessage("CustomerId value is invalid");
            RuleFor(x => x.BankAccountId).GreaterThan(0).WithMessage("BankAccountId value is invalid");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount should be greater than $0");
        }
    }
}
