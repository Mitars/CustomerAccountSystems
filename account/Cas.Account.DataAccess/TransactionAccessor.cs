using Cas.Account.Core.Dtos;
using Cas.Account.Core.Errors;
using Cas.Account.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cas.Account.DataAccess
{
    public class TransactionAccessor : ITransactionAccessor
    {
        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public TransactionAccessor(IConfiguration configuration)
        {
            this.httpClient = new HttpClient { BaseAddress = new Uri(configuration.GetSection("GatewayEndpoint").Value) };
            this.httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            this.jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
        }

        public async Task<List<TransactionDetailsDto>> GetByAccountId(long id)
        {
            var response = await this.httpClient.GetAsync($"/api/accounts/{id}/transactions");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<TransactionDetailsDto>();
            }
            else if (!response.IsSuccessStatusCode)
            {
                throw new NotFoundException(new { Account = "Not Found"});
            }

            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TransactionDetailsDto>>(result, jsonSerializerOptions);
        }

        public async Task<TransactionDetailsDto> Create(TransactionToCreateDto transaction)
        {
            var response = await this.httpClient.PostAsync("/api/transactions", new StringContent(JsonSerializer.Serialize(transaction), UnicodeEncoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed creating the transaction");
            }

            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TransactionDetailsDto>(result, jsonSerializerOptions);
        }
    }
}
