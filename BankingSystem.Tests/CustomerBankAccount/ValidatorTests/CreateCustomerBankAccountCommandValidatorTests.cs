using BankingSystem.WebApi.CustomerBankAccount.Application.Commands;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace BankingSystem.Tests.CustomerBankAccount.ValidatorTests
{
    public class CreateCustomerBankAccountCommandValidatorTests
    {
        private CreateCustomerBankAccountCommand _command = null!;
        private CreateCustomerBankAccountCommandValidator _validator = null!;

        public CreateCustomerBankAccountCommandValidatorTests()
        {
            _validator = new CreateCustomerBankAccountCommandValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData("123456789")]
        [InlineData("123-456789")]
        public void CreateCustomerBankAccountCommand_SsnIsNotSet_ThrowsException(string ssn)
        {
            _command = new CreateCustomerBankAccountCommand(ssn, "rad", "test", 1234);
            var result = _validator.TestValidate(_command);
            result.ShouldHaveValidationErrorFor(i => i.Ssn).WithErrorMessage("SSN should be specified in XXX-XX-XXXX format");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(99)]
        public void CreateCustomerBankAccountCommand_DepositAmountLessThan100_ThrowsException(decimal amount)
        {
            _command = new CreateCustomerBankAccountCommand("122-11-1111", "rad", "test", amount);
            var result = _validator.TestValidate(_command);
            result.ShouldHaveValidationErrorFor(i => i.DepositAmount).WithErrorMessage("Amount cannot be less than $100");
        }

        [Fact]
        public void CreateCustomerBankAccountCommand_DepositAmountMoreThan15000_ThrowsException()
        {
            _command = new CreateCustomerBankAccountCommand("122-11-1111", "rad", "test", 15000);
            var result = _validator.TestValidate(_command);
            result.ShouldHaveValidationErrorFor(i => i.DepositAmount).WithErrorMessage("Amount cannot be greater than $10,000");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CreateCustomerBankAccountCommand_FirstNameEmpty_ThrowsException(string firstName)
        {
            _command = new CreateCustomerBankAccountCommand("122-11-1111", firstName, "test", 15000);
            var result = _validator.TestValidate(_command);
            result.ShouldHaveValidationErrorFor(i => i.FirstName).WithErrorMessage("Please specify a first name");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CreateCustomerBankAccountCommand_LastNameEmpty_ThrowsException(string lastName)
        {
            _command = new CreateCustomerBankAccountCommand("122-11-1111", "rad", lastName, 15000);
            var result = _validator.TestValidate(_command);
            result.ShouldHaveValidationErrorFor(i => i.LastName).WithErrorMessage("Please specify a last name");
        }

        [Fact]
        public void CreateCustomerBankAccountCommand_Valid_Success()
        {
            _command = new CreateCustomerBankAccountCommand("122-11-1111", "rad", "test", 10000);
            var result = _validator.TestValidate(_command);
            result.IsValid.Should().BeTrue();
        }
    }
}