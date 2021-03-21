using Cas.Account.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Cas.Account.DataAccess
{
    public class CustomerAccessor : ICustomerAccessor
    {
        private readonly HttpClient httpClient;

        public CustomerAccessor(IConfiguration configuration)
        {
            this.httpClient = new HttpClient { BaseAddress = new Uri(configuration.GetSection("CustomerServiceEndpoint").Value) };
            this.httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> Exists(long id)
        {
            var response = await this.httpClient.GetAsync($"/Customers/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
            else if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not retrieve the customer info");
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
