using System.Collections.Generic;
using Cas.Customer.Api.DataAccess;
using Cas.Customer.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cas.Customer.Api.Controllers
{
    /// <summary>
    /// Class CustomerController.
    /// Implements the <see cref="ControllerBase" />
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        /// <summary>
        /// The customer repository instance.
        /// </summary>
        private readonly CustomerDataAccess customerRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomersController"/> class.
        /// </summary>
        /// <param name="customerRepository">The customer repository.</param>
        public CustomersController(CustomerDataAccess customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        /// <summary>
        /// Gets all the customers.
        /// </summary>
        /// <returns>The list of all customers.</returns>
        [HttpGet]
        public ActionResult<List<CustomerModel>> Get() =>
            this.customerRepository.Get();

        /// <summary>
        /// Gets a customer by their ID.
        /// </summary>
        /// <param name="id">The customer ID.</param>
        /// <returns>The customer.</returns>
        [HttpGet("{id}")]
        public ActionResult<CustomerModel> Get(long id) =>
            this.customerRepository.Get(id);
    }
}
