using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Cas.Transaction.Tests
{
    public class TransactionTests : TestBase
    {
        [Fact]
        public async Task Get_NonExistantTransaction_ReturnsNotFoundResponse()
        {
            var verifyTransactionDoesNotExistResponse = await this.Client.GetAsync($"/transactions/1");

            Assert.True(verifyTransactionDoesNotExistResponse.StatusCode == HttpStatusCode.NotFound, "Did not respond with not found for non-existant entry");
        }

        [Fact]
        public async Task Get_RetrieveTransactionsByAccountWithoutTransactions_NotFound()
        {
            var verifyTransactionExistsResponse = await this.Client.GetAsync($"/transactions?accountId=1");

            Assert.True(verifyTransactionExistsResponse.StatusCode == HttpStatusCode.NotFound, "Did not respond with not found for non-existant entry");
        }

        [Fact]
        public async Task Get_RetrieveTransactionsByAccountId_ReturnsTransactions()
        {
            var transaction1 = new { accountId = 1, amount = 10, description = "test deposit 1 on account 1" };
            var transaction2 = new { accountId = 2, amount = 10, description = "test deposit 1 on account 2" };
            var transaction3 = new { accountId = 2, amount = 10, description = "test deposit 2 on account 2" };

            await this.Client.PostAsJsonAsync("/transactions", transaction1);
            await this.Client.PostAsJsonAsync("/transactions", transaction2);
            await this.Client.PostAsJsonAsync("/transactions", transaction3);
            var verifyTransactionExistsResponse = await this.Client.GetAsync($"/transactions?accountId=2");

            Assert.True(verifyTransactionExistsResponse.IsSuccessStatusCode, "Failed to fetch newly created transaction");

            var transactions = await verifyTransactionExistsResponse.Content.ReadAsStringAsync();
            Assert.True(transactions.Contains("\"description\":\"test deposit 1 on account 2\"") && transactions.Contains("\"description\":\"test deposit 2 on account 2\""), "Created transaction does do not match expected values");
        }

        [Fact]
        public async Task Get_CreateAndRetrieveTransaction_ReturnsNewlyCreatedTransaction()
        {
            var transaction = new { accountId = 1, amount = 10, description = "test deposit" };

            var createdTransactionResponse = await this.Client.PostAsJsonAsync("/transactions", transaction);
            var verifyTransactionExistsResponse = await this.Client.GetAsync($"/transactions/1");

            var createdTransaction = await createdTransactionResponse.Content.ReadAsStringAsync();
            var retrivedTransaction = await verifyTransactionExistsResponse.Content.ReadAsStringAsync();

            Assert.True(createdTransactionResponse.IsSuccessStatusCode, "Failed creating transaction");
            Assert.True(verifyTransactionExistsResponse.IsSuccessStatusCode, "Failed to fetch newly created transaction");

            Assert.True(createdTransaction.StartsWith("{\"id\":1,\"amount\":10,\"description\":\"test deposit\""), "Created transaction does do not match expected values");
            Assert.True(retrivedTransaction.StartsWith("{\"id\":1,\"amount\":10,\"description\":\"test deposit\""), "Retrieved transaction does do not match expected values");
        }
    }
}
