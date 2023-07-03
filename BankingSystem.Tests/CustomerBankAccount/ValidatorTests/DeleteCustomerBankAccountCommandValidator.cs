using BankingSystem.WebApi.CustomerBankAccount.Application.Commands;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace BankingSystem.Tests.CustomerBankAccount.ValidatorTests
{
    public class DeleteCustomerBankAccountCommandValidatorTests
    {
        private DeleteCustomerBankAccountCommand _command = null!;
        private DeleteCustomerBankAccountCommandValidator _validator = null!;

        public DeleteCustomerBankAccountCommandValidatorTests()
        {
            _validator = new DeleteCustomerBankAccountCommandValidator();
        }

        [Fact]
        public void DeleteCustomerBankAccountCommand_CustomerIdNotSpecified_ThrowsException()
        {
            _command = new DeleteCustomerBankAccountCommand(0, 1);
            var result = _validator.TestValidate(_command);
            result.ShouldHaveValidationErrorFor(i => i.CustomerId).WithErrorMessage("CustomerId value is invalid");
        }

        [Fact]
        public void DeleteCustomerBankAccountCommand_BankAccountIdNotSpecified_ThrowsException()
        {
            _command = new DeleteCustomerBankAccountCommand(1, 0);
            var result = _validator.TestValidate(_command);
            result.ShouldHaveValidationErrorFor(i => i.BankAccountId).WithErrorMessage("BankAccountId value is invalid");
        }

        [Fact]
        public void DeleteCustomerBankAccountCommand_Valid_Success()
        {
            _command = new DeleteCustomerBankAccountCommand(1, 1);
            var result = _validator.TestValidate(_command);
            result.IsValid.Should().BeTrue();
        }
    }
}