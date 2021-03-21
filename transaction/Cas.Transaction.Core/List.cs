using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cas.Transaction.Core.Dtos;
using Cas.Transaction.Core.Errors;
using Cas.Transaction.Core.Interfaces;
using MediatR;

namespace Cas.Transaction.Core
{
    public static class List
    {
        public record Query : IRequest<List<TransactionDetailsDto>>
        {
            public long AccountId { get; init; }
        }
        
        public class Handler : IRequestHandler<Query, List<TransactionDetailsDto>>
        {
            private readonly ITransactionAccessor transactionAccessor;

            public Handler(ITransactionAccessor transactionAccessor)
            {
                this.transactionAccessor = transactionAccessor;
            }

            public async Task<List<TransactionDetailsDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var transactions = await this.transactionAccessor.GetByAccountId(request.AccountId);
                if (!transactions?.Any() ?? true)
                {
                    throw new NotFoundException(new { Transactions = "Not Found" });
                }

                return transactions.Select(transaction =>
                    new TransactionDetailsDto
                    {
                        Id = transaction.Id,
                        Amount = transaction.Amount,
                        Description = transaction.Description,
                        Created = transaction.Created,
                    }).ToList();
            }
        }
    }
}
