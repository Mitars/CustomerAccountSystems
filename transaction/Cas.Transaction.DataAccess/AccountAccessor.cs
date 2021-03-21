using Cas.Transaction.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Cas.Transaction.DataAccess
{
    public class AccountAccessor : IAccountAccessor
    {
        private readonly HttpClient httpClient;

        public AccountAccessor(IConfiguration configuration)
        {
            this.httpClient = new HttpClient { BaseAddress = new Uri(configuration.GetSection("GatewayEndpoint").Value) };
            this.httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> Exists(long id)
        {
            var response = await this.httpClient.GetAsync($"/api/accounts/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
            else if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not retrieve the account info");
            }

            var result = await response.Content.ReadAsStringAsync();
            if (result == null || result == "{}" || result == "[]")
            {
                return false;
            }

            return true;
        }
    }
}
