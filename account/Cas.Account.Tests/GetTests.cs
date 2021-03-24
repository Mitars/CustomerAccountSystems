using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Cas.Account.Tests
{
    public class GetTests : TestBase
    {
        [Fact]
        public async Task Get_NonExistantAccount_NotFoundResponse()
        {
            var response = await this.Client.GetAsync("/accounts/0");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_RetrieveAccountByCustomerId_ReturnsAccounts()
        {
            var account1 = new { customerId = 1, name = "Test Account - For Customer 1" };
            var account2 = new { customerId = 2, name = "Test Account 1 - For Customer 2" };
            var account3 = new { customerId = 2, name = "Test Account 2 - For Customer 2" };

            await this.Client.PostAsJsonAsync("/accounts", account1);
            await this.Client.PostAsJsonAsync("/accounts", account2);
            await this.Client.PostAsJsonAsync("/accounts", account3);
            var verifyAccountsExistsResponse = await this.Client.GetAsync("/accounts?customerId=2");

            Assert.True(verifyAccountsExistsResponse.IsSuccessStatusCode, "Failed to fetch newly created accounts");

            var accounts = await verifyAccountsExistsResponse.Content.ReadAsStringAsync();
            Assert.True(accounts.Contains("\"name\":\"Test Account 1 - For Customer 2\"") && accounts.Contains("\"name\":\"Test Account 2 - For Customer 2\""), "Created account does do not match expected values");
        }

        [Fact]
        public async Task Post_CreateAccount_ReturnsNewlyCreatedAccount()
        {
            var account = new { customerId = 1, name = "Test Account" };

            await this.Client.PostAsJsonAsync("/accounts", account);
            var retrievedAccountResponse = await this.Client.GetAsync("/accounts/1");
            var retrievedAccount = await retrievedAccountResponse.Content.ReadAsStringAsync();

            Assert.True(retrievedAccount.StartsWith("{\"id\":1,\"name\":\"Test Account\""), "Created accounts does do not match expected values");
        }

        [Fact]
        public async Task Get_CreateAndRetrieveAccount_ReturnsNewlyCreatedAccount()
        {
            var account = new { customerId = 1, name = "Test Account" };

            var createdAccountResponse = await this.Client.PostAsJsonAsync("/accounts", account);
            var verifyAccountExistsResponse = await this.Client.GetAsync("/accounts/1");

            var createdAccount = await createdAccountResponse.Content.ReadAsStringAsync();
            var retrivedAccount = await verifyAccountExistsResponse.Content.ReadAsStringAsync();

            Assert.True(createdAccountResponse.IsSuccessStatusCode, "Failed creating account");
            Assert.True(verifyAccountExistsResponse.IsSuccessStatusCode, "Failed to fetch newly created account");

            Assert.True(createdAccount.StartsWith("{\"id\":1,\"name\":\"Test Account\""), "Created account does do not match expected values");
            Assert.True(retrivedAccount.StartsWith("{\"id\":1,\"name\":\"Test Account\""), "Retrieved account does do not match expected values");
        }
    }
}
