using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Cas.Account.Tests
{
    public class CreateTests : TestBase
    {
        [Fact]
        public async Task Post_CreateAccount_ReturnsNewlyCreatedAccount()
        {
            var account = new { customerId = 1, name = "Test Account" };

            var response = await this.Client.PostAsJsonAsync("/accounts", account);
            var createdAccount = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode, "Failed creating accounts");
            Assert.True(createdAccount.StartsWith("{\"id\":1,\"name\":\"Test Account\""), "Created accounts does do not match expected values");
        }

        [Fact]
        public async Task Post_CreateAccount_ReturnsAccount()
        {
            var account = new { customerId = 1, name = "Test Account" };

            var response = await this.Client.PostAsJsonAsync("/accounts", account);

            Assert.True(response.IsSuccessStatusCode, "Failed to fetch newly created account");
        }

        [Fact]
        public async Task Post_CreateAccountWithNonExistantCustomer_NotFoundResponse()
        {
            var account = new { customerId = 0, name = "Test Account" };

            var response = await this.Client.PostAsJsonAsync("/accounts", account);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Post_CreateAccountWithMissingName_BadRequest()
        {
            var account = new { customerId = 1 };

            var response = await this.Client.PostAsJsonAsync("/accounts", account);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Post_CreateAccountWithMissingCustomerId_BadRequest()
        {
            var account = new { name = "Test Account" };

            var response = await this.Client.PostAsJsonAsync("/accounts", account);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Post_CreateAccountWithASpecifiedNonPositiveInitialCredit_BadRequest()
        {
            var account = new { customerId = 1, name = "Test Account", initialCredit = 0 };

            var response = await this.Client.PostAsJsonAsync("/accounts", account);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Post_CreateAccountWithInitialCredit_ReturnsAccount()
        {
            var account = new { customerId = 1, name = "Test Account", initialCredit = 500 };

            var response = await this.Client.PostAsJsonAsync("/accounts", account);
            var accountJson = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode, "Failed to fetch newly created account");
            Assert.True(accountJson.Contains("\"transactions\":[{\"id\":1,\"amount\":500"), "Failed to fetch newly created account");
        }

        [Fact]
        public async Task Post_CreateAccountWithInitialCreditAndRetrieveTheResults_ReturnsAccount()
        {
            var account = new { customerId = 1, name = "Test Account", initialCredit = 500 };

            await this.Client.PostAsJsonAsync("/accounts", account);
            var response = await this.Client.GetAsync("/accounts/1");
            var accountJson = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode, "Failed to fetch newly created account");
            Assert.True(accountJson.Contains("\"transactions\":[{\"id\":1,\"amount\":500"), "Failed to fetch newly created account");
        }
    }
}
