using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cas.Account.Core.Dtos;
using Cas.Account.Core.Errors;
using Cas.Account.Core.Interfaces;
using FluentValidation;
using MediatR;

namespace Cas.Account.Core
{
    public static class Create
    {
        public record Command : IRequest<AccountDetailsDto>
        {
            public long CustomerId { get; init; }
            public string Name { get; init; }
            public float? InitialCredit { get; init; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.CustomerId).NotEmpty();
                RuleFor(c => c.Name).NotEmpty();
                RuleFor(c => c.InitialCredit).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Command, AccountDetailsDto>
        {
            private readonly ICustomerAccessor customerAccessor;
            private readonly IAccountAccessor userAccessor;
            private readonly ITransactionAccessor transactionAccessor;

            public Handler(ICustomerAccessor customerAccessor, IAccountAccessor userAccessor, ITransactionAccessor transactionAccessor)
            {
                this.customerAccessor = customerAccessor;
                this.userAccessor = userAccessor;
                this.transactionAccessor = transactionAccessor;
            }

            public async Task<AccountDetailsDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var customerExists = await this.customerAccessor.Exists(request.CustomerId);
                if (!customerExists)
                {
                    throw new NotFoundException(new { Customer = "Not Found" });
                }

                var accountToCreate = new AccountToCreateDto
                {
                    CustomerId = request.CustomerId,
                    Name = request.Name
                };

                var createdAccount = await this.userAccessor.Create(accountToCreate);
                if (createdAccount == null)
                {
                    throw new Exception("Problem creating account");
                }

                var transactions = new List<TransactionDetailsDto>();
                if (request.InitialCredit != null && request.InitialCredit != 0)
                {
                    var transactionToCreate = new TransactionToCreateDto
                    {
                        Amount = (float)request.InitialCredit,
                        AccountId = createdAccount.Id,
                        Description = "Automated Message - Initial Transaction"
                    };
                    transactions.Add(await this.transactionAccessor.Create(transactionToCreate));
                }

                return new AccountDetailsDto
                {
                    Id = createdAccount.Id,
                    Name = createdAccount.Name,
                    Created = createdAccount.Created,
                    Transactions = transactions
                };
            }
        }
    }
}
