using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cas.Account.Core.Dtos;
using Cas.Account.Core.Errors;
using Cas.Account.Core.Interfaces;
using MediatR;

namespace Cas.Account.Core
{
    public static class List
    {
        public record Query : IRequest<List<AccountDetailsDto>>
        {
            public long CustomerId { get; init; }
        }

        public class Handler : IRequestHandler<Query, List<AccountDetailsDto>>
        {
            private readonly IAccountAccessor accountAccessor;
            private readonly ITransactionAccessor transactionAccessor;

            public Handler(IAccountAccessor accountAccessor, ITransactionAccessor transactionAccessor)
            {
                this.accountAccessor = accountAccessor;
                this.transactionAccessor = transactionAccessor;
            }

            public async Task<List<AccountDetailsDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var accounts = await this.accountAccessor.GetByCustomerId(request.CustomerId);
                if (accounts == null)
                {
                    throw new NotFoundException(new { Account = "Not found" });
                }

                return accounts.Select(account =>
                    new AccountDetailsDto()
                    {
                        Id = account.Id,
                        Name = account.Name,
                        Created = account.Created,
                        Transactions = this.transactionAccessor.GetByAccountId(account.Id).Result,
                    }).ToList();
            }
        }
    }
}
