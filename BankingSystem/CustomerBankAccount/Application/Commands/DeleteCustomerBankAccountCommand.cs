using FluentValidation;
using MediatR;

namespace BankingSystem.WebApi.CustomerBankAccount.Application.Commands
{
    public record DeleteCustomerBankAccountCommand(long CustomerId, long BankAccountId) : IRequest<bool>;

    public class DeleteCustomerBankAccountCommandValidator : AbstractValidator<DeleteCustomerBankAccountCommand>
    {
        public DeleteCustomerBankAccountCommandValidator()
        {
            RuleFor(x => x.CustomerId).GreaterThan(0).WithMessage("CustomerId value is invalid");
            RuleFor(x => x.BankAccountId).GreaterThan(0).WithMessage("BankAccountId value is invalid");
        }
    }
}
