using System;

namespace Cas.Customer.Api.Models
{
    /// <summary>
    /// The customer class.
    /// </summary>
    public record CustomerModel
    {
        public long Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public DateTime DateOfBirth { get; init; }
        public string Email { get; init; }
        public string Phone { get; init; }
        public string Address { get; init; }
        public string Company { get; init; }
        public string Gender { get; init; }
        public string Photo { get; set; }
        public DateTime Registered { get; init; }
    }
}
