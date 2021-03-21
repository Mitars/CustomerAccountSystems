using System;

namespace Cas.Account.Core.Dtos
{
    public record TransactionDetailsDto
    {
        public long Id { get; init; }
        public float Amount { get; init; }
        public string Description { get; init; }
        public DateTime Created { get; init; }
    }
}
