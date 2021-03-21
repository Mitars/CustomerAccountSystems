using System;

namespace Cas.Account.Core.Dtos
{
    public record TransactionToCreateDto
    {
        public long AccountId { get; init; }
        public float Amount { get; init; }
        public string Description { get; init; }
    }
}
