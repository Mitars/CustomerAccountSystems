using System;

namespace Cas.Transaction.Core.Models
{
    public class TransactionModel
    {
        public long Id { get; set; }
        public long AccountId { get; init; }
        public float Amount { get; init; }
        public string Description { get; init; }
        public DateTime Created { get; set; }
    }
}
