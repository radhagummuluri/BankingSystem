# BankingSystem Banking System application
*System Design:* 
----------------------------------------------------------------------------
System allows for users and user accounts.
A user can have as many accounts as they want.
A user can create and delete accounts.
A user can deposit and withdraw from accounts.
An account cannot have less than $100 at any time in an account.
A user cannot withdraw more than 90% of their total balance from an account in a single transaction.
A user cannot deposit more than $10,000 in a single transaction.

*API endpoints*
----------------------------------------------------------------------------

1. [HTTPGet] `/api/customerbankaccount/{customerId}` Gets all bank accounts for a given customerId
2. [HttpGet] `/api/customerbankaccount/{customerId}/bankaccount/{bankAccountId}` Gets a bank account for a given customerId and bankAccountId
3. [HttpPost] `/api/customerbankaccount/create` Creates a BankAccount for a given customer. Returns the customerId and bankAccountId in the response if successful.
4. [HttpPost] `/api/customerbankaccount/deposit` Deposits the specified amount into the specified customer bank account.
5. [HttpPost] `/api/customerbankaccount/withdraw` Withdraws the specified amount from the specified customer bank account.
6. [HttpDelete] `/api/customerbankaccount/delete` Deletes a bank account for a customer.

*Setup instructions*
----------------------------------------------------------------------------
1. Clone the repository.
2. Navigate to the source directory and run the command `docker compose up`
3. In a browser navigate to the swagger link `http://localhost:5000/swagger/index.html` to test the endpoints.
