using System;
using System.Threading;
using System.Threading.Tasks;
using Cas.Transaction.Core.Dtos;
using Cas.Transaction.Core.Errors;
using Cas.Transaction.Core.Interfaces;
using FluentValidation;
using MediatR;

namespace Cas.Transaction.Core
{
    public static class Create
    {
        public record Command : IRequest<TransactionDetailsDto>
        {
            public long AccountId { get; init; }
            public string Description { get; init; }
            public float Amount { get; init; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Amount).NotEqual(0);
            }
        }

        public class Handler : IRequestHandler<Command, TransactionDetailsDto>
        {
            private readonly IAccountAccessor accountAccessor;
            private readonly ITransactionAccessor transactionAccessor;

            public Handler(IAccountAccessor accountAccessor, ITransactionAccessor transactionAccessor)
            {
                this.accountAccessor = accountAccessor;
                this.transactionAccessor = transactionAccessor;
            }

            public async Task<TransactionDetailsDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var accountExists = await this.accountAccessor.Exists(request.AccountId);
                if (!accountExists)
                {
                    throw new NotFoundException(new { Account = "Not Found" });
                }

                var transactionToCreate = new TransactionToCreateDto
                {
                    AccountId = request.AccountId,
                    Amount = request.Amount,
                    Description = request.Description,
                };

                var createdTransaction = await this.transactionAccessor.Create(transactionToCreate);
                if (createdTransaction == null)
                {
                    throw new Exception("Problem creating Transaction");
                }

                return new TransactionDetailsDto
                {
                    Id = createdTransaction.Id,
                    Amount = createdTransaction.Amount,
                    Description = createdTransaction.Description,
                    Created = createdTransaction.Created
                };
            }
        }
    }
}
