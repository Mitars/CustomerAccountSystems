using Cas.Customer.Api.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Cas.Customer.Api.DataAccess
{
    /// <summary>
    /// The customer repository class.
    /// Reads the customer information from a JSON file.
    /// </summary>
    public class CustomerDataAccess
    {
        private readonly JsonSerializerOptions jsonSerializerOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerDataAccess"/> class.
        /// </summary>
        public CustomerDataAccess()
        {
            this.jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
        }

        /// <summary>
        /// Gets all the customers.
        /// </summary>
        /// <returns>The list of all customers.</returns>
        public List<CustomerModel> Get()
        {
            var json = File.ReadAllText(Path.Combine("DataAccess", "customerData.json"));
            return JsonSerializer.Deserialize<List<CustomerModel>>(json, jsonSerializerOptions);
        }

        /// <summary>
        /// Gets a customer by their ID.
        /// </summary>
        /// <param name="id">The customer ID.</param>
        /// <returns>The customer.</returns>
        public CustomerModel Get(long id) =>
            this.Get().Find(customer => customer.Id == id);
    }
}
