using System.Threading;
using System.Threading.Tasks;
using Cas.Transaction.Core.Dtos;
using Cas.Transaction.Core.Errors;
using Cas.Transaction.Core.Interfaces;
using MediatR;

namespace Cas.Transaction.Core
{
    public static class Details
    {
        public record Query : IRequest<TransactionDetailsDto>
        {
            public long Id { get; init; }
        }
        
        public class Handler : IRequestHandler<Query, TransactionDetailsDto>
        {
            private readonly ITransactionAccessor transactionAccessor;

            public Handler(ITransactionAccessor transactionAccessor)
            {
                this.transactionAccessor = transactionAccessor;
            }

            public async Task<TransactionDetailsDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var transaction = await this.transactionAccessor.Get(request.Id);
                if (transaction == null)
                {
                    throw new NotFoundException(new { Transaction = "Not Found" });
                }

                return new TransactionDetailsDto
                    {
                        Id = transaction.Id,
                        Amount = transaction.Amount,
                        Description = transaction.Description,
                        Created = transaction.Created,
                    };
            }
        }
    }
}
