using Cas.Transaction.Core.Dtos;
using Cas.Transaction.Core.Interfaces;
using Cas.Transaction.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cas.Transaction.DataAccess
{
    public class TransactionAccessor : ITransactionAccessor
    {
        private readonly DataContext dataContext;

        public TransactionAccessor(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public Task<TransactionModel> Get(long id) =>
            this.dataContext.Transactions.FirstOrDefaultAsync(transaction => transaction.Id == id);

        public Task<List<TransactionModel>> GetByAccountId(long accountId) =>
            this.dataContext.Transactions
                .Where(transaction => transaction.AccountId == accountId)
                .OrderByDescending(transaction => transaction.Created)
                .ToListAsync();

        public async Task<TransactionModel> Create(TransactionToCreateDto transactionToCreate)
        {
            var transaction = new TransactionModel
            {
                AccountId = transactionToCreate.AccountId,
                Amount = transactionToCreate.Amount,
                Description = transactionToCreate.Description,
                Created = DateTime.UtcNow
            };

            await this.dataContext.Transactions.AddAsync(transaction);

            var success = await this.dataContext.SaveChangesAsync() > 0;
            if (!success)
            {
                throw new Exception("Problem saving changes");
            }

            return transaction;
        }
    }
}
