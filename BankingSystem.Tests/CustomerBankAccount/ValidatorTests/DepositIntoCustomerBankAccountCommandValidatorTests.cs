using BankingSystem.WebApi.CustomerBankAccount.Application.Commands;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace BankingSystem.Tests.CustomerBankAccount.ValidatorTests
{
    public class DepositIntoCustomerBankAccountCommandValidatorTests
    {
        private DepositIntoCustomerBankAccountCommand _command = null!;
        private DepositIntoCustomerBankAccountCommandValidator _validator = null!;

        public DepositIntoCustomerBankAccountCommandValidatorTests()
        {
            _validator = new DepositIntoCustomerBankAccountCommandValidator();
        }

        [Fact]
        public void DepositIntoCustomerBankAccountCommand_CustomerIdNotSpecified_ThrowsException()
        {
            _command = new DepositIntoCustomerBankAccountCommand(0, 1, 200);
            var result = _validator.TestValidate(_command);
            result.ShouldHaveValidationErrorFor(i => i.CustomerId).WithErrorMessage("CustomerId value is invalid");
        }

        [Fact]
        public void DepositIntoCustomerBankAccountCommand_BankAccountIdNotSpecified_ThrowsException()
        {
            _command = new DepositIntoCustomerBankAccountCommand(1, 0, 200);
            var result = _validator.TestValidate(_command);
            result.ShouldHaveValidationErrorFor(i => i.BankAccountId).WithErrorMessage("BankAccountId value is invalid");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(99)]
        public void DepositIntoCustomerBankAccountCommand_DepositAmountLessThan100_ThrowsException(decimal amount)
        {
            _command = new DepositIntoCustomerBankAccountCommand(1, 0, amount);
            var result = _validator.TestValidate(_command);
            result.ShouldHaveValidationErrorFor(i => i.Amount).WithErrorMessage("Amount cannot be less than $100");
        }

        [Fact]
        public void DepositIntoCustomerBankAccountCommand_DepositAmountMoreThan15000_ThrowsException()
        {
            _command = new DepositIntoCustomerBankAccountCommand(1, 0, 15000);
            var result = _validator.TestValidate(_command);
            result.ShouldHaveValidationErrorFor(i => i.Amount).WithErrorMessage("Amount cannot be greater than $10,000");
        }

        [Fact]
        public void DepositIntoCustomerBankAccountCommand_Valid_Success()
        {
            _command = new DepositIntoCustomerBankAccountCommand(1, 1, 10000);
            var result = _validator.TestValidate(_command);
            result.IsValid.Should().BeTrue();
        }
    }
}