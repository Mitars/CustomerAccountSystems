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

            var response = await this.Client.PostAsJsonAsync("/transactions", transaction);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
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

            var response = await this.Client.PostAsJsonAsync("/transactions", transaction);
            var createdTransaction = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode, "Failed creating transaction");
            Assert.True(createdTransaction.StartsWith("{\"id\":1,\"amount\":10,\"description\":\"test deposit\""), "Created transaction does do not match expected values");
        }

        [Fact]
        public async Task Get_RetrieveTransactionsByNonExistantAccountId_NotFound()
        {
            var transaction = new { accountId = 0, amount = 10, description = "test deposit 1 on account 1" };

            var response = await this.Client.PostAsJsonAsync("/transactions", transaction);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
