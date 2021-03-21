using System;

namespace Cas.Account.Core.Models
{
    public class AccountModel
    {
        public long Id { get; init; }
        public long CustomerId { get; init; }
        public string Name { get; init; }
        public DateTime Created { get; set; }
    }
}
