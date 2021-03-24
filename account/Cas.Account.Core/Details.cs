using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Cas.Account.Core.Dtos;
using Cas.Account.Core.Errors;
using Cas.Account.Core.Interfaces;
using MediatR;

namespace Cas.Account.Core
{
    public static class Details
    {
        public record Query : IRequest<AccountDetailsDto>
        {
            public long Id { get; init; }
        }

        public class Handler : IRequestHandler<Query, AccountDetailsDto>
        {
            private readonly IAccountAccessor accountAccessor;
            private readonly ITransactionAccessor transactionAccessor;

            public Handler(IAccountAccessor accountAccessor, ITransactionAccessor transactionAccessor)
            {
                this.accountAccessor = accountAccessor;
                this.transactionAccessor = transactionAccessor;
            }

            public async Task<AccountDetailsDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var account = await this.accountAccessor.Get(request.Id);
                if (account == null)
                {
                    throw new NotFoundException(new { account = "Not found" });
                }

                var transactions = await this.transactionAccessor.GetByAccountId(account.Id);

                return new AccountDetailsDto
                {
                    Id = account.Id,
                    Name = account.Name,
                    Created = account.Created,
                    Transactions = transactions,
                };
            }
        }
    }
}
