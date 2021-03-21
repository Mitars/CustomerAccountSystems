using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Cas.Transaction.Tests
{
    public class CreateTests : TestBase
    {
        [Fact]
        public async Task Post_CreateTransactionWithAmountZero_BadRequest()
        {
            var transaction = new { accountId = 1, amount = 0, description = "Invalid amount" };

            var createdTransactionResponse = await this.Client.PostAsJsonAsync("/transactions", transaction);

            Assert.True(createdTransactionResponse.StatusCode == HttpStatusCode.BadRequest, "Status code did not match expected Bad Request");
        }

        [Fact]
        public async Task Post_CreateTransaction_ReturnsTransaction()
        {
            var transaction = new { accountId = 1, amount = 10, description = "test deposit" };

            var response = await this.Client.PostAsJsonAsync("/transactions", transaction);

            Assert.True(response.IsSuccessStatusCode, "Failed to fetch newly created transaction");
        }

        [Fact]
        public async Task Post_CreateTransaction_ReturnsNewlyCreatedTransaction()
        {
            var transaction = new { accountId = 1, amount = 10, description = "test deposit" };

            var createdTransactionResponse = await this.Client.PostAsJsonAsync("/transactions", transaction);
            var createdTransaction = await createdTransactionResponse.Content.ReadAsStringAsync();
            
            Assert.True(createdTransactionResponse.IsSuccessStatusCode, "Failed creating transaction");
            Assert.True(createdTransaction.StartsWith("{\"id\":1,\"amount\":10,\"description\":\"test deposit\""), "Created transaction does do not match expected values");
        }

        [Fact]
        public async Task Get_RetrieveTransactionsByNonExistantAccountId_NotFound()
        {
            var transaction = new { accountId = 0, amount = 10, description = "test deposit 1 on account 1" };

            var response = await this.Client.PostAsJsonAsync("/transactions", transaction);

            Assert.True(response.StatusCode == HttpStatusCode.NotFound, "Did not respond with not found for non-existant entry");
        }
    }
}
