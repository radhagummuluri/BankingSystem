using BankingSystem.WebApi.Common.Constants;
using BankingSystem.WebApi.CustomerBankAccount.Contracts;
using FluentValidation;
using MediatR;

namespace BankingSystem.WebApi.CustomerBankAccount.Application.Commands
{
    public record CreateCustomerBankAccountCommand(string Ssn, string FirstName, string LastName, decimal DepositAmount) : IRequest<CreatedBankAccountResponse>;

    public class CreateCustomerBankAccountCommandValidator : AbstractValidator<CreateCustomerBankAccountCommand>
    {
        public CreateCustomerBankAccountCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Please specify a first name");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Please specify a last name");
            RuleFor(x => x.Ssn).Matches(ValidationRuleConstants.SSN_REGEX).WithMessage(ValidationRuleConstants.SSN_VALIDATION_MESSAGE);
            RuleFor(x => x.DepositAmount)
                .LessThanOrEqualTo(10000)
                .WithMessage("Amount cannot be greater than $10,000")
                .GreaterThanOrEqualTo(100)
                .WithMessage("Amount cannot be less than $100");
        }
    }
}
