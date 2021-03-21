using System;
using System.Collections.Generic;

namespace Cas.Account.Core.Dtos
{
    public class AccountDetailsDto
    {
        public long Id { get; init; }
        public string Name { get; init; }
        public DateTime Created { get; set; }
        public List<TransactionDetailsDto> Transactions { get; set; }
    }
}
